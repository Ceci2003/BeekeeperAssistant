namespace BeekeeperAssistant.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
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
        private readonly IConfiguration configuration;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
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

            //var cloudinaryAccount = this.configuration.GetSection("Cloudinary");

            Account account = new Account(
                this.configuration["Cloudinary:CloudName"],
                this.configuration["Cloudinary:APIKey"],
                this.configuration["Cloudinary:APISecret"]);
                //cloudinaryAccount["CloudName"],
                //cloudinaryAccount["APIKey"],
                //cloudinaryAccount["APISecret"]

            Cloudinary cloudinary = new Cloudinary(account);

            var file = this.Input.ImageFile;

            var uploadResult = new ImageUploadResult();

            var imageUrl = "";

            if (file != null)
            {
                if (file.Length > 0)
                {
                    string fileExtension = Path.GetExtension(file.FileName);

                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription($"{user.Id}{fileExtension}", stream),
                            Overwrite = true,
                            Folder = "beekeeper_assistant",
                            PublicId = $"profile/{user.Id}",
                            //PublicId = $"beekeeper_assistant/{user.Id}.{fileExtension}",
                            //Transformation = new Transformation().Width(100).Height(100).Gravity("face").Radius("max").Border("2px_solid_white").Crop("thumb"),
                        };

                        uploadResult = cloudinary.Upload(uploadParams);
                    }
                }

                imageUrl = uploadResult.Uri.ToString();
            }
            else
            {
                imageUrl = this.Input.ImageUrl;
            }

            user.ProfileImageUrl = imageUrl;
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
