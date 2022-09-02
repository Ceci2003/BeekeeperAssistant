﻿namespace BeekeeperAssistant.Services.Messaging
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class NullMessageSender : IEmailSender
    {
        public Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null)
        {
            return Task.CompletedTask;
        }

        public Task SendMultipleEmailsAsync(
            string from,
            string fromName,
            string[] emails,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null)
        {
            return Task.CompletedTask;
        }
    }
}
