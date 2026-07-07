using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

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

            // روش کاملاً سازگار با تمام نسخه‌های 5 بدون دستکاری مسیر
            IMediaInfo mediaInfo;
            try
            {
                mediaInfo = await FFmpeg.GetMediaInfo(tempPath);
            }
            catch
            {
                // اگر خطا داد یعنی فایل اجرایی ffmpeg وجود ندارد، پس در پوشه Temp دانلود می‌کند
                await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Full);
                mediaInfo = await FFmpeg.GetMediaInfo(tempPath);
            }

            if (mediaInfo.Duration.TotalSeconds > 60)
            {
                System.IO.File.Delete(tempPath);
                throw new Exception("مدت زمان ویدیو نباید بیشتر از ۱ دقیقه باشد.");
            }

            IConversion conversion = FFmpeg.Conversions.New()
                .AddParameter($"-i \"{tempPath}\"")
                .AddParameter("-c:v libx264")       // کدک ویدیو H264
                .AddParameter("-c:a aac")           // کدک صدا AAC
                .AddParameter("-b:v 500k")          // بیت ریت 500 کیلوبایت بر ثانیه
                .AddParameter("-vf scale=1280:720") // تغییر رزولوشن به 720p
                .SetOutput(finalPath)
                .SetOverwriteOutput(true);

            await conversion.Start();

            if (System.IO.File.Exists(tempPath))
            {
                System.IO.File.Delete(tempPath);
            }

            return $"/{folderPath}/{finalFileName}";
        }
    }
}