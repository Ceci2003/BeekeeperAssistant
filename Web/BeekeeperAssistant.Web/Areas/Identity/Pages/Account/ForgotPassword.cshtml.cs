namespace BeekeeperAssistant.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Messaging;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Configuration;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class ForgotPasswordModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;

        public ForgotPasswordModel(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Полето 'Имейл' е задължително!")]
            [EmailAddress]
            [Display(Name = "Имейл")]
            public string Email { get; set; }
        }

#pragma warning disable SA1201 // Elements should appear in the correct order
        public async Task<IActionResult> OnPostAsync()
#pragma warning restore SA1201 // Elements should appear in the correct order
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userManager.FindByEmailAsync(this.Input.Email);
                if (user == null || !(await this.userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return this.RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await this.userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = this.Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: this.Request.Scheme);

                await this.emailSender.SendEmailAsync(
                    this.configuration["SendGrid:SenderEmail"],
                    GlobalConstants.SystemName,
                    this.Input.Email,
                    "Промяна на парола",
                    $"Последвайте линка за да промените паролата си: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'><strong>Смяна на паролата</strong></a>.");

                return this.RedirectToPage("./ForgotPasswordConfirmation");
            }

            return this.Page();
        }
    }
}
