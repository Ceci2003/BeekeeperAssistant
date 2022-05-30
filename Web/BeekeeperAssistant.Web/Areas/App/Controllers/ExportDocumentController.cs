namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.ExportDocument;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ExportDocumentController : AppBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApiaryService apiaryService;
        private readonly IExportDocumentService exportDocumentService;

        public ExportDocumentController(
            UserManager<ApplicationUser> userManager,
            IApiaryService apiaryService,
            IExportDocumentService exportDocumentService)
        {
            this.userManager = userManager;
            this.apiaryService = apiaryService;
            this.exportDocumentService = exportDocumentService;
        }

        public async Task<IActionResult> ExcelExportForDFZ(int? id)
        {
            var inputModel = new CreateExcelExportForDFZInputModel();

            if (id == null)
            {
                var currentuser = await this.userManager.GetUserAsync(this.User);
                inputModel.AllApiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentuser.Id);
            }
            else
            {
                inputModel.ApiaryId = id.Value;
            }

            inputModel.InventoryDate = DateTime.UtcNow.Date;
            inputModel.SubmissionDate = DateTime.UtcNow.Date;

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> ExcelExportForDFZ(int? id, CreateExcelExportForDFZInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                if (!id.HasValue)
                {
                    inputModel.AllApiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
                }

                return this.View(inputModel);
            }

            var result = this.exportDocumentService.ExcelExportForDFZ(inputModel);

            //this.TempData[GlobalConstants.SuccessMessage] = $"Експортирането е успешно. Моля изчакайте изтеглянето на файла.";

            this.Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReportDFZ.xlsx");

            return new FileContentResult(result.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
