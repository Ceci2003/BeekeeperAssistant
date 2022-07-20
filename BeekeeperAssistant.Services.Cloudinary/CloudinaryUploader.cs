namespace BeekeeperAssistant.Services.Cloudinary
{
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Service to upload images to Cloudinary.
    /// </summary>
    public class CloudinaryUploader : ICloudinaryUploader
    {
        private readonly Account account;
        private readonly Cloudinary cloudinary;

        public CloudinaryUploader(
            string cloudName,
            string apiKey,
            string apiSecret)
        {
            this.account = new Account(
                cloudName,
                apiKey,
                apiSecret);

            this.cloudinary = new Cloudinary(this.account);
        }

        public async Task<string> UploadImageAsync(string userId, IFormFile file, string folder, string publicId, bool overwrite = false)
        {
            var uploadResult = new ImageUploadResult();

            if (file == null)
            {
                return null;
            }

            if (file.Length <= 0)
            {
                return null;
            }

            string fileExtension = Path.GetExtension(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription($"{userId}{fileExtension}", stream),
                    Overwrite = overwrite,
                    Folder = "beekeeper_assistant",
                    PublicId = $"{folder}/{publicId}",
                };

                uploadResult = await this.cloudinary.UploadAsync(uploadParams);
            }

            var url = uploadResult.Url.ToString();

            return url;
        }
    }
}
