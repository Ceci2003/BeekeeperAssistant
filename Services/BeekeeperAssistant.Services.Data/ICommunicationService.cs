namespace BeekeeperAssistant.Services.Data
{
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface ICommunicationService
    {
        Task SendEmail(
            string from,
            string fromName,
            SendEmailOptions sendOptions,
            string to,
            string toMultiple,
            string subject,
            string htmlContent);
    }
}
