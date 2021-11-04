namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Text;

    using OfficeOpenXml;

    public interface IExcelExportService
    {
        ExcelPackage ExportAsExcelApiary(string userId);

        ExcelPackage ExportAsExcelBeehive(string userId, int? id);
    }
}
