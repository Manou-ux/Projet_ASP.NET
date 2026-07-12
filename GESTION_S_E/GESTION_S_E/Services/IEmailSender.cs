using System.Threading.Tasks;

namespace GESTION_S_E.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}