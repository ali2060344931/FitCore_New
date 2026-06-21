using FitCore.Application.Services.Auth;

using GymBot.Models;

using Microsoft.Extensions.Configuration;

using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymBot.Services
{
    public class BaleBotService : IBaleBotService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;

        // تغییر اینجا: دریافت مستقیم HttpClient به جای IHttpClientFactory
        public BaleBotService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _token = configuration["BaleBot:Token"];
        }

        public async Task<bool> SendMessageAsync(long chatId, string text)
        {
            var url = $"https://tapi.bale.ai/bot{_token}/sendMessage";
            var payload = new BaleSendMessageRequest
            {
                ChatId = chatId,
                Text = text
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            return response.IsSuccessStatusCode;
        }
    }
}