namespace BeekeeperAssistant.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Cloudinary;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ICloudinaryUploader cloudinaryUploader;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICloudinaryUploader cloudinaryUploader)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.cloudinaryUploader = cloudinaryUploader;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Телефонен номер")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Връзка към снимката")]
            public string ImageUrl { get; set; }

            public IFormFile ImageFile { get; set; }
        }

#pragma warning disable SA1201 // Elements should appear in the correct order
        private async Task LoadAsync(ApplicationUser user)
#pragma warning restore SA1201 // Elements should appear in the correct order
        {
            var userName = await this.userManager.GetUserNameAsync(user);
            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);

            this.Username = userName;

            this.Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                ImageUrl = user.ProfileImageUrl,
            };
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

        public async Task<IActionResult> OnPostAsync()
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

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    this.StatusMessage = "Unexpected error when trying to set phone number.";
                    return this.RedirectToPage();
                }
            }

            var file = this.Input.ImageFile;

            if (file != null)
            {
                var url = await this.cloudinaryUploader.UploadImageAsync(user.Id, file, "profile", user.Id, true);

                if (!string.IsNullOrEmpty(url))
                {
                    user.ProfileImageUrl = url;
                }
                else
                {
                    user.ProfileImageUrl = this.Input.ImageUrl;
                }
            }
            else
            {

                user.ProfileImageUrl = this.Input.ImageUrl;
            }

            var setImageResult = await this.userManager.UpdateAsync(user);
            if (!setImageResult.Succeeded)
            {
                this.StatusMessage = "Unexpected error when trying to set phone number.";
                return this.RedirectToPage();
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }
    }
}
