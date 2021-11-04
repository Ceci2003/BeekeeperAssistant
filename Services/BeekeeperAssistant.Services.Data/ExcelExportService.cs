namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using BeekeeperAssistant.Services.Data.Models;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    public class ExcelExportService : IExcelExportService
    {
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;

        public ExcelExportService(IApiaryService apiaryService, IBeehiveService beehiveService)
        {
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
        }

        public ExcelPackage ExportAsExcelApiary(string userId)
        {
            var apiaries = this.apiaryService.GetAllUserApiaries<ApiaryDataExcelExportModel>(userId);

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1:B1"].Merge = true;
            ws.Cells["A1"].Value = "Доклад - Кошери";
            ws.Cells["A2:B2"].Merge = true;
            ws.Cells["A2"].Value = $"Дата: {string.Format("{0:dd-MM-yyyy} {0:H:mm}", DateTimeOffset.Now)}";

            ws.Cells["A4"].Value = "Номер";
            ws.Cells["B4"].Value = "Адрес";
            ws.Cells["C4"].Value = "Име";
            ws.Cells["D4"].Value = "Вид";

            // ws.Cells["E4"].Value = "Брой кошери";
            ws.Cells["A4:E4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A4:E4"].Style.Fill.BackgroundColor.SetColor(1, 183, 225, 205);
            ws.Cells["A4:E4"].Style.Font.Color.SetColor(Color.White);

            int rowIndex = 5;
            foreach (var apiary in apiaries)
            {
                ws.Cells[$"A{rowIndex}"].Value = apiary.Number;
                ws.Cells[$"B{rowIndex}"].Value = apiary.Adress;
                ws.Cells[$"C{rowIndex}"].Value = apiary.Name == null ? "-" : apiary.Name;
                ws.Cells[$"D{rowIndex}"].Value = apiary.ApiaryType;

                // ws.Cells[$"D{rowIndex}"].Value = apiary.Beehives.ToList().Count();
                rowIndex++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();

            return pck;
        }

        public ExcelPackage ExportAsExcelBeehive(string userId, int? id)
        {
            IEnumerable<BeehiveDataExcelExportModel> beehives =
                id.HasValue ?
                this.beehiveService.GetBeehivesByApiaryId<BeehiveDataExcelExportModel>(id.Value) :
                this.beehiveService.GetAllUserBeehives<BeehiveDataExcelExportModel>(userId);

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1:B1"].Merge = true;
            ws.Cells["A1"].Value = "Доклад - Кошери";
            ws.Cells["A2:B2"].Merge = true;
            ws.Cells["A2"].Value = $"Дата: {string.Format("{0:dd-MM-yyyy} {0:H:mm}", DateTimeOffset.Now)}";

            ws.Cells["A4"].Value = "Номер на кошера";
            ws.Cells["B4"].Value = "Създаден на";
            ws.Cells["C4"].Value = "Номер на пчелин";
            ws.Cells["D4"].Value = "Подвижен";
            ws.Cells["E4"].Value = "Сила";
            ws.Cells["F4"].Value = "Система";
            ws.Cells["G4"].Value = "Тип";
            ws.Cells["H4"].Value = "Прашецоуловител";
            ws.Cells["I4"].Value = "Решетка за прополис";

            ws.Cells["J4"].Value = "Кралица";
            ws.Cells["K4"].Value = "Кралица-Цвят";
            ws.Cells["L4"].Value = "Кралица-Дата на придаване";
            ws.Cells["M4"].Value = "Кралица-Произход";
            ws.Cells["N4"].Value = "Кралица-Вид";
            ws.Cells["O4"].Value = "Кралица-Порода";
            ws.Cells["P4"].Value = "Кралица-Нрав";
            ws.Cells["Q4"].Value = "Кралица-Хигиенни навици";

            ws.Cells["A4:Q4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A4:Q4"].Style.Fill.BackgroundColor.SetColor(1, 183, 225, 205);
            ws.Cells["A4:Q4"].Style.Font.Color.SetColor(Color.White);
            ws.Cells["A4:Q4"].Style.Font.Bold = true;

            int rowIndex = 5;
            foreach (var beehive in beehives)
            {
                ws.Cells[$"A{rowIndex}"].Value = beehive.Number;
                ws.Cells[$"B{rowIndex}"].Value = beehive.Date.ToString("dd-MM-yyyy");
                ws.Cells[$"C{rowIndex}"].Value = beehive.ApiaryNumber;
                ws.Cells[$"D{rowIndex}"].Value = beehive.IsItMovable == true ? "✓	" : string.Empty;

                ws.Cells[$"D{rowIndex}"].Style.Font.Bold = true;

                ws.Cells[$"E{rowIndex}"].Value = beehive.BeehivePower;
                ws.Cells[$"F{rowIndex}"].Value = beehive.BeehiveSystem;
                ws.Cells[$"G{rowIndex}"].Value = beehive.BeehiveType;
                ws.Cells[$"H{rowIndex}"].Value = beehive.HasPolenCatcher == true ? "Да" : "Не";
                ws.Cells[$"I{rowIndex}"].Value = beehive.HasPropolisCatcher == true ? "Да" : "Не";
                if (beehive.HasQueen)
                {
                    // Color color = Color.White;
                    // if (beehive.Queen.Color != )
                    // {
                    //    switch (beehive.Queen.Color)
                    //    {
                    //        case QueenColor.White: color = Color.White; break;
                    //        case QueenColor.Yellow: color = Color.Yellow; break;
                    //        case QueenColor.Red: color = Color.Red; break;
                    //        case QueenColor.Green: color = Color.Green; break;
                    //        case QueenColor.Blue: color = Color.Blue; break;
                    //        default: color = Color.White; break;
                    //    }
                    // }

                    // ws.Cells[$"J{rowIndex}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    // ws.Cells[$"J{rowIndex}"].Style.Fill.BackgroundColor.SetColor(1, 183, 225, 205);
                    ws.Cells[$"J{rowIndex}"].Value = beehive.HasQueen ? "Да" : "Не";
                    ws.Cells[$"K{rowIndex}"].Value = beehive.QueenColor.ToString();
                    ws.Cells[$"L{rowIndex}"].Value = beehive.QueenGivingDate.Value.ToString("dd-MM-yyyy");
                    ws.Cells[$"M{rowIndex}"].Value = beehive.QueenOrigin;
                    ws.Cells[$"N{rowIndex}"].Value = beehive.QueenQueenType;
                    ws.Cells[$"O{rowIndex}"].Value = beehive.QueenBreed;
                    ws.Cells[$"P{rowIndex}"].Value = beehive.QueenTemperament;
                    ws.Cells[$"Q{rowIndex}"].Value = beehive.QueenHygenicHabits;
                }

                rowIndex++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();

            return pck;
        }
    }
}
