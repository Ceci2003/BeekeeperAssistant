using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using BeekeeperAssistant.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using BeekeeperAssistant.Common;
using Microsoft.Extensions.Configuration;
using BeekeeperAssistant.Services.Messaging;

namespace BeekeeperAssistant.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration configuration;

        public EmailModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._emailSender = emailSender;
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

        private async Task LoadAsync(ApplicationUser user)
        {
            var email = await this._userManager.GetEmailAsync(user);
            this.Email = email;

            this.Input = new InputModel
            {
                NewEmail = email,
            };

            this.IsEmailConfirmed = await this._userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var email = await this._userManager.GetEmailAsync(user);
            if (this.Input.NewEmail != email)
            {
                var userId = await this._userManager.GetUserIdAsync(user);
                var code = await this._userManager.GenerateChangeEmailTokenAsync(user, this.Input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = this.Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId = userId, email = this.Input.NewEmail, code = code },
                    protocol: this.Request.Scheme);


                await this._emailSender.SendEmailAsync(
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
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var userId = await this._userManager.GetUserIdAsync(user);
            var email = await this._userManager.GetEmailAsync(user);
            var code = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = this.Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: this.Request.Scheme);

            await this._emailSender.SendEmailAsync(
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
