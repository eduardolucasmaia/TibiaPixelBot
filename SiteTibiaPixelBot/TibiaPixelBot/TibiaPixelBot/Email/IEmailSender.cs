using System.Threading.Tasks;

namespace TibiaPixelBot.Email
{
    public interface IEmailSender
    {
        Task EnviarEmailAsync(string subject, string body, string to);
    }
}
