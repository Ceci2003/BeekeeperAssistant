namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Messaging;
    using Microsoft.AspNetCore.Http;

    public class CommunicationService : ICommunicationService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IEmailSender emailSender;

        public CommunicationService(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IEmailSender emailSender)
        {
            this.userRepository = userRepository;
            this.emailSender = emailSender;
        }

        public async Task SendEmail(
            string from,
            string fromName,
            SendEmailOptions sendOptions,
            string to,
            string toMultiple,
            string subject,
            string htmlContent)
        {
            if (sendOptions == SendEmailOptions.ToEveryone)
            {
                var usersEmails = this.userRepository.All().Select(u => u.Email).ToArray();
                foreach (var email in usersEmails)
                {
                    await this.emailSender.SendEmailAsync(from, fromName, email, subject, htmlContent);
                }
            }
            else if (sendOptions == SendEmailOptions.ToAllConfirmedEmails)
            {
                var usersEmails = this.userRepository.All().Where(u => u.EmailConfirmed == true).Select(u => u.Email).ToArray();
                await this.emailSender.SendMultipleEmailsAsync(from, fromName, usersEmails, subject, htmlContent);
                foreach (var email in usersEmails)
                {
                    await this.emailSender.SendEmailAsync(from, fromName, email, subject, htmlContent);
                }
            }
            else if (sendOptions == SendEmailOptions.ToOne)
            {
                await this.emailSender.SendEmailAsync(from, fromName, to, subject, htmlContent);
            }
            else if (sendOptions == SendEmailOptions.ToMultiple)
            {
                var usersEmails = toMultiple.Split(';', System.StringSplitOptions.RemoveEmptyEntries);

                if (usersEmails.Length > 1)
                {
                    foreach (var email in usersEmails)
                    {
                        await this.emailSender.SendEmailAsync(from, fromName, email, subject, htmlContent);
                    }
                }
                else
                {
                    await this.emailSender.SendEmailAsync(from, fromName, usersEmails[0], subject, htmlContent);
                }
            }
        }
    }
}
