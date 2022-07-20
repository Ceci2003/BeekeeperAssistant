namespace BeekeeperAssistant.Services.Cloudinary
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Service to upload images to Cloudinary.
    /// </summary>
    public interface ICloudinaryUploader
    {
        /// <summary>
        /// Uploads an image to Cloudinary identified with a specified user.
        /// </summary>
        /// <param name="userId">Id is used to associate the image with the user.</param>
        /// <param name="file">Sets the actual data of the image.</param>
        /// <param name="folder">Specified folder to place the image.</param>
        /// <param name="publicId">Sets the identifier that is used for accessing the uploaded resource. A randomly generated id is assigned if not specified.</param>
        /// <param name="overwrite">Sets whether to overwrite existing resources with the same public ID.</param>
        /// <returns>Returns a string with the URL of the image.</returns>
        Task<string> UploadImageAsync(
            string userId,
            IFormFile file,
            string folder,
            string publicId,
            bool overwrite = false);
    }
}
