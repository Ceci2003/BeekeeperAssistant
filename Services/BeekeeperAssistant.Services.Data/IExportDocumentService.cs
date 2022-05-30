namespace BeekeeperAssistant.Services
{
    using BeekeeperAssistant.Web.ViewModels.ExportDocument;
    using OfficeOpenXml;

    public interface IExportDocumentService
    {
        ExcelPackage ExcelExportForDFZ(CreateExcelExportForDFZInputModel inputModel);
    }
}
