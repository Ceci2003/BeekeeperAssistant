namespace BeekeeperAssistant.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Messaging;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Configuration;

    public partial class EmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;

        public EmailModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.configuration = configuration;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }
        }

#pragma warning disable SA1201 // Elements should appear in the correct order
        private async Task LoadAsync(ApplicationUser user)
#pragma warning restore SA1201 // Elements should appear in the correct order
        {
            var email = await this.userManager.GetEmailAsync(user);
            this.Email = email;

            this.Input = new InputModel
            {
                NewEmail = email,
            };

            this.IsEmailConfirmed = await this.userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var email = await this.userManager.GetEmailAsync(user);
            if (this.Input.NewEmail != email)
            {
                var userId = await this.userManager.GetUserIdAsync(user);
                var code = await this.userManager.GenerateChangeEmailTokenAsync(user, this.Input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = this.Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId = userId, email = this.Input.NewEmail, code = code },
                    protocol: this.Request.Scheme);


                await this.emailSender.SendEmailAsync(
                    this.configuration["SendGrid:SenderEmail"],
                    GlobalConstants.SystemName,
                    this.Input.NewEmail,
                    "Потвърждване на имейла",
                    $"Регистрацията е успешна!<br>ПоследваЙте линка за да потвърдете имейл адреса си: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'><strong>Потвърди имейл</strong></a>.");

                this.StatusMessage = "Изпратихме ви имейл за потвърждение.";
                return this.RedirectToPage();
            }

            this.StatusMessage = "Имейлът не е променен.";
            return this.RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var userId = await this.userManager.GetUserIdAsync(user);
            var email = await this.userManager.GetEmailAsync(user);
            var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = this.Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: this.Request.Scheme);

            await this.emailSender.SendEmailAsync(
                this.configuration["SendGrid:SenderEmail"],
                GlobalConstants.SystemName,
                email,
                "Потвърждване на имейла",
                $"Регистрацията е успешна!<br>ПоследваЙте линка за да потвърдете имейл адреса си: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'><strong>Потвърди имейл</strong></a>.");

            this.StatusMessage = "Изпратихме ви имейл за потвърждение.";
            return this.RedirectToPage();
        }
    }
}
