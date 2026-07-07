using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
// ---- این ۳ خط فراموش شده بودند و باعث خطا شدند ----
using ImageMagick;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace FitCore.Application.Services
{
    public interface IFileCompressionService
    {
        Task<string> SaveAndCompressImageAsync(IFormFile file, string folderPath);
        Task<string> SaveAndCompressVideoAsync(IFormFile file, string folderPath);
    }

    public class FileCompressionService : IFileCompressionService
    {
        private readonly IWebHostEnvironment _env;

        public FileCompressionService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveAndCompressImageAsync(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0) return null;

            var uploadsFolder = Path.Combine(_env.WebRootPath, folderPath);
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            using (var image = new MagickImage(filePath))
            {
                if (image.Width > 800) image.Resize(800, 0);
                image.Quality = 80;
                image.Strip();
                await image.WriteAsync(filePath);
            }

            return $"/{folderPath}/{uniqueFileName}";
        }

        public async Task<string> SaveAndCompressVideoAsync(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0) return null;

            var uploadsFolder = Path.Combine(_env.WebRootPath, folderPath);
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var tempFileName = Guid.NewGuid() + "_temp" + Path.GetExtension(file.FileName);
            var finalFileName = Guid.NewGuid() + ".mp4";

            var tempPath = Path.Combine(uploadsFolder, tempFileName);
            var finalPath = Path.Combine(uploadsFolder, finalFileName);

            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // مسیر دقیق و مستقیم فایل اجرایی در پوشه سایت
            var ffmpegPath = Path.Combine(_env.WebRootPath, "FFmpeg", "ffmpeg.exe");

            if (!System.IO.File.Exists(ffmpegPath))
            {
                if (System.IO.File.Exists(tempPath)) System.IO.File.Delete(tempPath);
                throw new Exception("فایل اجرایی FFmpeg در پوشه wwwroot/FFmpeg پیدا نشد.");
            }

            // 1. استخراج مدت زمان ویدیو برای چک کردن 1 دقیقه
            var durationCheck = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = $"-i \"{tempPath}\" -hide_banner",
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            durationCheck.Start();
            string errorOutput = await durationCheck.StandardError.ReadToEndAsync();
            durationCheck.WaitForExit();

            // پیدا کردن مدت زمان با Regex از خروجی کنسول
            var match = Regex.Match(errorOutput, @"Duration: (\d{2}):(\d{2}):(\d{2}\.\d{2})");
            if (match.Success)
            {
                int hours = int.Parse(match.Groups[1].Value);
                int minutes = int.Parse(match.Groups[2].Value);
                double seconds = double.Parse(match.Groups[3].Value);
                double totalSeconds = hours * 3600 + minutes * 60 + seconds;

                if (totalSeconds > 60)
                {
                    if (System.IO.File.Exists(tempPath)) System.IO.File.Delete(tempPath);
                    throw new Exception("مدت زمان ویدیو نباید بیشتر از ۱ دقیقه باشد.");
                }
            }

            // 2. فشرده‌سازی ویدیو با دستورات مستقیم
            var conversion = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = $"-i \"{tempPath}\" -c:v libx264 -c:a aac -b:v 500k -vf scale=1280:720 -y \"{finalPath}\"",
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            conversion.Start();
            // خواندن خروجی برای جلوگیری از هنگ کردن پردازش
            await conversion.StandardError.ReadToEndAsync();
            conversion.WaitForExit();

            if (conversion.ExitCode != 0)
            {
                if (System.IO.File.Exists(tempPath)) System.IO.File.Delete(tempPath);
                if (System.IO.File.Exists(finalPath)) System.IO.File.Delete(finalPath);
                throw new Exception("خطا در فشرده‌سازی ویدیو. فرمت فایل نامعتبر است.");
            }

            // 3. پاک کردن فایل موقت اصلی
            if (System.IO.File.Exists(tempPath))
            {
                System.IO.File.Delete(tempPath);
            }

            return $"/{folderPath}/{finalFileName}";
        }
    }
}