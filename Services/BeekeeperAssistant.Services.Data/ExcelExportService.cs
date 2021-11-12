namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data.Models;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    public class ExcelExportService : IExcelExportService
    {
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;
        private readonly IHarvestService harvestService;
        private readonly IInspectionService inspectionService;
        private readonly ITreatmentService treatmentService;

        public ExcelExportService(
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            IHarvestService harvestService,
            IInspectionService inspectionService,
            ITreatmentService treatmentService)
        {
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
            this.harvestService = harvestService;
            this.inspectionService = inspectionService;
            this.treatmentService = treatmentService;
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

        public ExcelPackage ExportAsExcelHarvest(int id)
        {
            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataExcelExportModel>(id);
            var harvests = this.harvestService.GetAllBeehiveHarvests<HarvestDataExcelExportModel>(id);

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1:B1"].Merge = true;
            ws.Cells["A1"].Value = $"Доклад - Добиви";

            ws.Cells["A2"].Value = $"Пчелин №:";
            ws.Cells["B2"].Value = $"{beehive.ApiaryNumber}";
            ws.Cells["A3"].Value = $"Кошер №:";
            ws.Cells["B3"].Value = $"{beehive.Number}";

            ws.Cells["A4:B4"].Merge = true;
            ws.Cells["A4"].Value = $"Дата: {string.Format("{0:dd-MM-yyyy} {0:H:mm}", DateTimeOffset.Now)}";

            ws.Cells[$"A6"].Value = $"Име";
            ws.Cells[$"B6"].Value = $"Дата";
            ws.Cells[$"C6"].Value = $"Добит продукт";
            ws.Cells[$"D6"].Value = $"Вид мед";
            ws.Cells[$"E6"].Value = $"Количество";
            ws.Cells[$"F6"].Value = $"Еденица";
            ws.Cells[$"G6"].Value = $"Бележка";

            ws.Cells["A6:G6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A6:G6"].Style.Fill.BackgroundColor.SetColor(1, 183, 225, 205);
            ws.Cells["A6:G6"].Style.Font.Color.SetColor(Color.White);
            ws.Cells["A6:G6"].Style.Font.Bold = true;

            int rowsCounter = 7;
            foreach (var harvest in harvests)
            {
                ws.Cells[$"A{rowsCounter}"].Value = harvest.HarvestName;
                ws.Cells[$"B{rowsCounter}"].Value = harvest.DateOfHarves.ToString("dd-MM-yyyy");

                var harvestedProduct = "мед";
                switch (harvest.HarvestProductType)
                {
                    case HarvestProductType.Pollen:
                        harvestedProduct = "прашец";
                        break;
                    case HarvestProductType.Wax:
                        harvestedProduct = "восък";
                        break;
                    case HarvestProductType.Propolis:
                        harvestedProduct = "прополис";
                        break;
                    case HarvestProductType.RoyalJelly:
                        harvestedProduct = "млечице";
                        break;
                    case HarvestProductType.BeeVenom:
                        harvestedProduct = "отрова";
                        break;
                }

                ws.Cells[$"C{rowsCounter}"].Value = harvestedProduct;

                var honeyType = string.Empty;
                if (harvest.HarvestProductType == HarvestProductType.Honey)
                {
                    switch (harvest.HoneyType)
                    {
                        case HoneyType.Acacia:
                            honeyType = "акация";
                            break;
                        case HoneyType.Wildflower:
                            honeyType = "билков";
                            break;
                        case HoneyType.Sunflower:
                            honeyType = "слънчогледов";
                            break;
                        case HoneyType.Clover:
                            honeyType = "от детелина";
                            break;
                        case HoneyType.Alfalfa:
                            honeyType = "от люцерна";
                            break;
                        case HoneyType.Other:
                            honeyType = "друг";
                            break;
                    }
                }

                ws.Cells[$"D{rowsCounter}"].Value = honeyType;
                ws.Cells[$"E{rowsCounter}"].Value = harvest.Quantity;

                var unit = "";
                switch (harvest.Unit)
                {
                    case Unit.Kilograms:
                        unit = "кг";
                        break;
                    case Unit.Grams:
                        unit = "г";
                        break;
                    case Unit.Milligrams:
                        unit = "мг";
                        break;
                    case Unit.Litres:
                        unit = "л";
                        break;
                    case Unit.Millilitres:
                        unit = "мл";
                        break;
                }

                ws.Cells[$"F{rowsCounter}"].Value = unit;
                ws.Cells[$"G{rowsCounter}"].Value = harvest.Note;

                rowsCounter++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();

            return pck;
        }

        public ExcelPackage ExportAsExcelInspection(int id)
        {
            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataExcelExportModel>(id);
            var inspection = this.inspectionService.GetAllBeehiveInspections<InspectionDataExcelExportModel>(id).FirstOrDefault();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = $"Дата на прегледа:";
            ws.Cells["B1"].Value = DateTime.Now.ToString("dd-MM-yyyy");
            ws.Cells["A2"].Value = "Вид на прегледа:";
            ws.Cells["B2"].Value = inspection.InspectionType;
            ws.Cells["A3"].Value = "Роил ли се е:";
            ws.Cells["B3"].Value = inspection.Swarmed;
            ws.Cells["A4"].Value = "Сила на кошера:";
            ws.Cells["B4"].Value = inspection.BeehivePower;

            ws.Cells["D2"].Value = $"Пчелин №:";
            ws.Cells["E2"].Value = $"{beehive.ApiaryNumber}";
            ws.Cells["D3"].Value = $"Кошер №:";
            ws.Cells["E3"].Value = $"{beehive.Number}";

            ws.Cells["A4:B4"].Merge = true;
            ws.Cells["A4"].Value = $"Дата: {string.Format("{0:dd-MM-yyyy} {0:H:mm}", DateTimeOffset.Now)}";

            ws.Cells["A6:B6"].Merge = true;
            ws.Cells["A6:B6"].Style.Font.Bold = true;
            ws.Cells["A6:B6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A6"].Value = "Основна информация:";
            ws.Cells["A7"].Value = "Предприети действия:";
            ws.Cells["B7"].Value = inspection.BeeActivity;
            ws.Cells["A8"].Value = "Маса на кошера(кг.):";
            ws.Cells["B8"].Value = inspection.Weight;
            ws.Cells["A9"].Value = "Температура на кошера(t°):";
            ws.Cells["B9"].Value = inspection.HiveTemperature;
            ws.Cells["A10"].Value = "Влажност на кошера(%):";
            ws.Cells["B10"].Value = inspection.HiveHumidity;

            if (!inspection.IncludeQueenSection)
            {
                for (int i = 12; i < 17; i++)
                {
                    ws.Row(i).Hidden = true;
                }
            }

            ws.Cells["A12:B12"].Merge = true;
            ws.Cells["A12:B12"].Style.Font.Bold = true;
            ws.Cells["A12:B12"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A12"].Value = "Секция майка";
            ws.Cells["A13"].Value = "Забелязана майка:";
            ws.Cells["B13"].Value = inspection.QueenSeen;
            ws.Cells["A14"].Value = "Маточници:";
            ws.Cells["B14"].Value = inspection.QueenCells;
            ws.Cells["A15"].Value = "Работен статус на майката:";
            ws.Cells["B15"].Value = inspection.QueenWorkingStatus;
            ws.Cells["A16"].Value = "Сила на майката:";
            ws.Cells["B16"].Value = inspection.QueenPowerStatus;

            if (!inspection.IncludeBrood)
            {
                for (int i = 18; i < 22; i++)
                {
                    ws.Row(i).Hidden = true;
                }
            }

            ws.Cells["A18:B18"].Merge = true;
            ws.Cells["A18:B18"].Style.Font.Bold = true;
            ws.Cells["A18:B18"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A18"].Value = "Секция пило";
            ws.Cells["A19"].Value = "Яйца:";
            ws.Cells["B19"].Value = inspection.Eggs;
            ws.Cells["A20"].Value = "Запечатано:";
            ws.Cells["B20"].Value = inspection.ClappedBrood;
            ws.Cells["A21"].Value = "Излюпващо се:";
            ws.Cells["B21"].Value = inspection.UnclappedBrood;

            if (!inspection.IncludeFramesWith)
            {
                for (int i = 23; i < 28; i++)
                {
                    ws.Row(i).Hidden = true;
                }
            }

            ws.Cells["A23:B23"].Merge = true;
            ws.Cells["A23:B23"].Style.Font.Bold = true;
            ws.Cells["A23:B23"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A23"].Value = "Секция пити";
            ws.Cells["A24"].Value = "С пчели:";
            ws.Cells["B24"].Value = inspection.FramesWithBees;
            ws.Cells["A25"].Value = "С пило:";
            ws.Cells["B25"].Value = inspection.FramesWithBrood;
            ws.Cells["A26"].Value = "С мед:";
            ws.Cells["B26"].Value = inspection.FramesWithHoney;
            ws.Cells["A27"].Value = "С Прашец:";
            ws.Cells["B27"].Value = inspection.FramesWithPollen;

            if (!inspection.IncludeActivity)
            {
                for (int i = 29; i < 35; i++)
                {
                    ws.Row(i).Hidden = true;
                }
            }

            ws.Cells["A29:B29"].Merge = true;
            ws.Cells["A29:B29"].Style.Font.Bold = true;
            ws.Cells["A29:B29"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A29"].Value = "Секция активност";
            ws.Cells["A30"].Value = "Активност на пчелите:"; ws.Cells["B30"].Value = inspection.BeeActivity;
            ws.Cells["A31"].Value = "Ориентационни полети:"; ws.Cells["B31"].Value = inspection.OrientationActivity;
            ws.Cells["A32"].Value = "Принос на прашец:"; ws.Cells["B32"].Value = inspection.PollenActivity;
            ws.Cells["A33"].Value = "Принос на мед:"; ws.Cells["B33"].Value = inspection.ForragingActivity;
            ws.Cells["A34"].Value = "Пчели в минута:"; ws.Cells["B34"].Value = inspection.BeesPerMinute;

            if (!inspection.IncludeStorage)
            {
                for (int i = 36; i < 39; i++)
                {
                    ws.Row(i).Hidden = true;
                }
            }

            ws.Cells["A36:B36"].Merge = true;
            ws.Cells["A36:B36"].Style.Font.Bold = true;
            ws.Cells["A36:B36"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A36"].Value = "Секция запаси";
            ws.Cells["A37"].Value = "От мед:";
            ws.Cells["B37"].Value = inspection.StoredHoney;
            ws.Cells["A38"].Value = "От прашец:";
            ws.Cells["B38"].Value = inspection.StoredPollen;

            if (!inspection.IncludeSpottedProblem)
            {
                for (int i = 40; i < 45; i++)
                {
                    ws.Row(i).Hidden = true;
                }
            }

            ws.Cells["A40:B40"].Merge = true;
            ws.Cells["A40:B40"].Style.Font.Bold = true;
            ws.Cells["A40:B40"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A40"].Value = "Секция проблеми";
            ws.Cells["A41"].Value = "Проблем:"; 
            ws.Cells["B41"].Value = inspection.Disease;
            ws.Cells["A42"].Value = "Третиране с:"; 
            ws.Cells["B42"].Value = inspection.Treatment;
            ws.Cells["A43"].Value = "Вредители:";
            ws.Cells["B43"].Value = inspection.Pests;
            ws.Cells["A44"].Value = "Хищници:"; 
            ws.Cells["B44"].Value = inspection.Predators;

            if (!inspection.IncludeWeatherInfo)
            {
                for (int i = 46; i < 50; i++)
                {
                    ws.Row(i).Hidden = true;
                }
            }

            ws.Cells["A46:B46"].Merge = true;
            ws.Cells["A46:B46"].Style.Font.Bold = true;
            ws.Cells["A46:B46"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A46"].Value = "Секция метеорологични";
            ws.Cells["A47"].Value = "Условия";
            ws.Cells["B47"].Value = inspection.Conditions;
            ws.Cells["A48"].Value = "Температура(t°):";
            ws.Cells["B48"].Value = inspection.WeatherTemperature;
            ws.Cells["A49"].Value = "Влажност(%):";
            ws.Cells["B49"].Value = inspection.WeatherHumidity;

            ws.Cells["A51:B51"].Merge = true;
            ws.Cells["A51:B51"].Style.Font.Bold = true;
            ws.Cells["A51:B51"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A51"].Value = "Бележка";

            ws.Cells["A52:B52"].Merge = true;
            ws.Cells["A52:B52"].Style.Font.Bold = true;
            ws.Cells["A52:B52"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A52:B52"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells["A52"].Value = inspection.Note;

            ws.Cells["B:B"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws.Cells["A:AZ"].AutoFitColumns();

            return pck;
        }

        public ExcelPackage ExportAsExcelTreatment(int id)
        {
            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataExcelExportModel>(id);
            var treatments = this.treatmentService.GetAllBeehiveTreatments<TreatmentDataExcelExportModel>(id);

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1:B1"].Merge = true;
            ws.Cells["A1"].Value = $"Доклад - Третирания";

            ws.Cells["A2"].Value = $"Пчелин №:";
            ws.Cells["B2"].Value = $"{beehive.ApiaryNumber}";
            ws.Cells["A3"].Value = $"Кошер №:";
            ws.Cells["B3"].Value = $"{beehive.Number}";

            ws.Cells["A4:B4"].Merge = true;
            ws.Cells["A4"].Value = $"Дата: {string.Format("{0:dd-MM-yyyy} {0:H:mm}", DateTimeOffset.Now)}";

            ws.Cells[$"A6"].Value = $"Име";
            ws.Cells[$"B6"].Value = $"Дата";
            ws.Cells[$"C6"].Value = $"Превенция на";
            ws.Cells[$"D6"].Value = $"Препарат";
            ws.Cells[$"E6"].Value = $"Въведен като";
            ws.Cells[$"F6"].Value = $"Количество";
            ws.Cells[$"G6"].Value = $"Дозировка";

            ws.Cells["A6:G6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A6:G6"].Style.Fill.BackgroundColor.SetColor(1, 183, 225, 205);
            ws.Cells["A6:G6"].Style.Font.Color.SetColor(Color.White);
            ws.Cells["A6:G6"].Style.Font.Bold = true;

            int rowsCounter = 7;
            foreach (var treatment in treatments)
            {
                ws.Cells[$"A{rowsCounter}"].Value = treatment.Name;
                ws.Cells[$"B{rowsCounter}"].Value = treatment.DateOfTreatment.ToString("dd-MM-yyyy");
                ws.Cells[$"C{rowsCounter}"].Value = treatment.Disease;
                ws.Cells[$"D{rowsCounter}"].Value = treatment.Medication;

                var inputAs = treatment.InputAs.ToString();
                switch (treatment.InputAs)
                {
                    case InputAs.PerHive:
                        inputAs = "на кошер";
                        break;
                    case InputAs.Total:
                        inputAs = "общо";
                        break;
                }

                ws.Cells[$"E{rowsCounter}"].Value = inputAs;
                ws.Cells[$"F{rowsCounter}"].Value = treatment.Quantity;

                var dose = treatment.Dose.ToString();
                switch (treatment.Dose)
                {
                    case Dose.Strips:
                        dose = "ленти";
                        break;
                    case Dose.Drops:
                        dose = "капки";
                        break;
                    case Dose.Grams:
                        dose = "г";
                        break;
                    case Dose.Kilograms:
                        dose = "кг";
                        break;
                    case Dose.Millilitres:
                        dose = "мл";
                        break;
                    case Dose.Litres:
                        dose = "л";
                        break;
                }

                ws.Cells[$"G{rowsCounter}"].Value = dose;

                rowsCounter++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();

            return pck;
        }
    }
}
