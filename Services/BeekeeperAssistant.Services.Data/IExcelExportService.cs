namespace BeekeeperAssistant.Services.Data
{
    using OfficeOpenXml;

    public interface IExcelExportService
    {
        ExcelPackage ExportAsExcelApiary(string userId);

        ExcelPackage ExportAsExcelBeehive(string userId, int? id);

        ExcelPackage ExportAsExcelHarvest(int id);

        ExcelPackage ExportAsExcelInspection(int id);

        ExcelPackage ExportAsExcelTreatment(int id);
    }
}
