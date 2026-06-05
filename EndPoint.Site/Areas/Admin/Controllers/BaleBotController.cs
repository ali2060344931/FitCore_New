using Microsoft.AspNetCore.Mvc;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [ApiController]
    [Route("api/bale")]
    public class BaleBotController : ControllerBase
    {
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] BaleUpdate update)
        {
            if (update?.Message != null)
            {
                var chatId = update.Message.Chat.Id;
                var text = update.Message.Text;

                await SendMessage(chatId, "پیام شما دریافت شد ✅");
            }

            return Ok();
        }

        private async Task SendMessage(long chatId, string text)
        {
            var token = "930538662:tybHHJEfdPiUPnKBhleM2pxn7SWERQh8hq4";
            var url = $"https://tapi.bale.ai/bot{token}/sendMessage";

            using var client = new HttpClient();

            var data = new
            {
                chat_id = chatId,
                text = text
            };

            await client.PostAsJsonAsync(url, data);
        }
    }
    public class BaleUpdate
    {
        public BaleMessage Message { get; set; }
    }
    public class BaleMessage
    {
        public long Message_Id { get; set; }
        public BaleChat Chat { get; set; }
        public string Text { get; set; }
    }
    public class BaleChat
    {
        public long Id { get; set; }
    }
}
