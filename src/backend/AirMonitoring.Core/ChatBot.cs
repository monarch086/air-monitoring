using Telegram.Bot.Types;
using Telegram.Bot;

namespace AirMonitoring.Core
{
    public class ChatBot
    {
        private TelegramBotClient client;

        public ChatBot(string token)
        {
            client = new TelegramBotClient(token);
        }

        public async Task Post(string message, int chatId)
        {
            var t = await client.SendTextMessageAsync(chatId, message, Telegram.Bot.Types.Enums.ParseMode.Html);
        }

        public async Task PostImage(string fileName, string text, int chatId)
        {
            Message message;

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var finalPath = Path.Combine(basePath, fileName);

            using (Stream stream = System.IO.File.OpenRead(finalPath))
            {
                message = await postImage(stream, text, chatId);
            }
        }

        public async Task PostImageBytes(byte[] buffer, string text, int chatId)
        {
            Message message;

            using (Stream stream = new MemoryStream(buffer))
            {
                message = await postImage(stream, text, chatId);
            }
        }

        private async Task<Message> postImage(Stream stream, string text, int chatId)
        {
            return await client.SendPhotoAsync(
                chatId: chatId,
                photo: stream,
                caption: text
            );
        }
    }
}
