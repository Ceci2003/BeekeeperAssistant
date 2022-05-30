namespace BeekeeperAssistant.Services
{
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Services.Data.Models;
    using BeekeeperAssistant.Web.ViewModels.ExportDocument;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using System.Linq;

    public class ExportDocumentService : IExportDocumentService
    {
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;

        public ExportDocumentService(
            IApiaryService apiaryService,
            IBeehiveService beehiveService)
        {
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
        }


        private const string EmptyFieldShort = ".............";
        private const string EmptyFieldMegaLong = ".........................................................................................................................";


        public ExcelPackage ExcelExportForDFZ(CreateExcelExportForDFZInputModel inputModel)
        {
            var apiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(inputModel.ApiaryId);
            var beehives = this.beehiveService.GetBeehivesByApiaryId<BeehiveDataExcelExportModel>(inputModel.ApiaryId);
            var beehivesCount = beehives.Count();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Export");

            int n = 1;

            ws.Cells[$"A{n}:I{n}"].Merge = true;
            ws.Cells[$"A{n}:I{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"A{n}:I{n}"].Style.Font.Size = 8;
            ws.Cells[$"A{n}:I{n}"].Value = "Образец ЗХОЖКФ – 87 А / Утвърден със Заповед № РД 11- 481/07.03.2019 г. на изпълнителния директор на БАБХ";

            n++; // n=2

            ws.Cells[$"A{n}:A{n + 6}"].Style.Font.Size = 10;
            ws.Cells[$"A{n}:A{n + 6}"].Style.Font.Bold = true;
            ws.Cells[$"A{n}"].Value = "ДО ОФИЦИАЛЕН ВЕТЕРИНАРЕН ЛЕКАР";
            ws.Cells[$"A{++n}"].Value = "НА ОБЩИНА " + inputModel.UserCity.ToUpper();
            ws.Cells[$"A{++n}"].Value = "ОБЛАСТ " + inputModel.UserMunicipality.ToUpper();
            ws.Cells[$"A{++n}"].Value = "ДО РЕГИСТРИРАН ВЕТЕРИНАРЕН ЛЕКАР";
            ws.Cells[$"A{++n}"].Value = "ДОГОВОР ЗА ОБСЛУЖВАНЕ №…………….";
            ws.Cells[$"A{++n}"].Value = "ОБЩИНА " + inputModel.UserCity.ToUpper();
            ws.Cells[$"A{++n}"].Value = "ОБЛАСТ " + inputModel.UserMunicipality.ToUpper();

            n += 3; // n=11 
            ws.Cells[$"A{n}:I{n}"].Style.Font.Size = 14;
            ws.Cells[$"A{n}:I{n}"].Style.Font.Bold = true;
            ws.Cells[$"A{n}:I{n}"].Merge = true;
            ws.Cells[$"A{n}:I{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"A{n}:I{n}"].Value = "Д Е К Л А Р А Ц И Я";

            n++; // n=12
            ws.Cells[$"A{n}:B{n}"].Merge = true;
            ws.Cells[$"A{n}:B{n}"].Value = "Подписаният";
            ws.Cells[$"C{n}:G{n}"].Merge = true;
            ws.Cells[$"C{n}:G{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"C{n}:G{n}"].Value = inputModel.UserNames;

            n++; // n=13
            ws.Cells[$"C{n}:G{n}"].Merge = true;
            ws.Cells[$"C{n}:G{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"C{n}:G{n}"].Style.Font.Italic = true;
            ws.Cells[$"C{n}:G{n}"].Style.Font.Size = 10;
            ws.Cells[$"C{n}:G{n}"].Value = "(име фамилия)";

            n++; // n=14
            ws.Cells[$"A{n}:I{n + 1}"].Merge = true;
            ws.Cells[$"A{n}:I{n + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[$"A{n}:I{n + 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            ws.Cells[$"A{n}:I{n + 1}"].Style.WrapText = true;

            string apartmentBuilding = "";
            if (string.IsNullOrWhiteSpace(inputModel.UserApartmentBuilding))
            {
                apartmentBuilding = EmptyFieldShort;
            }

            string apartment = "";
            if (string.IsNullOrWhiteSpace(inputModel.UserApartment))
            {
                apartment = EmptyFieldShort;
            }

            string floor = "";
            if (string.IsNullOrWhiteSpace(inputModel.UserFloor.ToString()))
            {
                floor = EmptyFieldShort;
            }

            ws.Cells[$"A{n}:I{n + 1}"].Value = $"с ЕГН/ЕИК: {inputModel.EgnEik}, с адрес: гр./с. {inputModel.UserCity} , Община {inputModel.UserMunicipality}, ул. {inputModel.UserStreet} №{inputModel.UserStreetNumber}," +
                                               $" бл. {apartmentBuilding}, ап. {apartment}, ет. {floor}, тел. {inputModel.PhoneNumber}";

            n += 4; // n=18
            ws.Cells[$"A{n}:B{n}"].Merge = true;
            ws.Cells[$"A{n}:B{n}"].Value = "В качеството си на ";

            ws.Cells[$"C{n}:G{n}"].Merge = true;
            ws.Cells[$"C{n}:G{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"C{n}:G{n}"].Value = inputModel.UserTypeAsFirst;

            n++; // n=19
            ws.Cells[$"A{n}:I{n + 1}"].Merge = true;
            ws.Cells[$"A{n}:I{n + 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            ws.Cells[$"A{n}:I{n + 1}"].Style.WrapText = true;
            ws.Cells[$"A{n}:I{n + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"A{n}:I{n + 1}"].Style.Font.Italic = true;
            ws.Cells[$"A{n}:I{n + 1}"].Style.Font.Size = 10;
            ws.Cells[$"A{n}:I{n + 1}"].Value = "(собственик/ползвател/управител, изпълнителен директор,съдружник, член на УС, член на борд на директорите и др.)";

            n += 2; // n=21
            ws.Cells[$"A{n}"].Value = "на";
            ws.Cells[$"B{n}:I{n}"].Merge = true;
            ws.Cells[$"B{n}:I{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"B{n}:I{n + 1}"].Value = inputModel.NameOfET;

            n++; // n=22
            ws.Cells[$"A{n}:I{n}"].Merge = true;
            ws.Cells[$"A{n}:I{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"A{n}:I{n}"].Style.Font.Italic = true;
            ws.Cells[$"A{n}:I{n}"].Style.Font.Size = 10;
            ws.Cells[$"A{n}:I{n}"].Value = "(наименование на юридическо лице/ЕТ)";

            n += 2; // n=24
            ws.Cells[$"A{n}:B{n}"].Merge = true;
            ws.Cells[$"A{n}:B{n}"].Value = "В качеството си на ";

            ws.Cells[$"C{n}:E{n}"].Merge = true;
            ws.Cells[$"C{n}:E{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"C{n}:E{n}"].Value = inputModel.UserTypeAsSecond;

            ws.Cells[$"F{n}:H{n}"].Merge = true;
            ws.Cells[$"F{n}:H{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"F{n}:H{n}"].Value = "на животновъден обект с №";

            ws.Cells[$"I{n}"].Merge = true;
            ws.Cells[$"I{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"I{n}"].Value = apiaryNumber;

            n++; // n=25
            ws.Cells[$"A{n}:I{n + 1}"].Merge = true;
            ws.Cells[$"A{n}:I{n + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[$"A{n}:I{n + 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            ws.Cells[$"A{n}:I{n + 1}"].Style.WrapText = true;
            ws.Cells[$"A{n}:I{n + 1}"].Value = $"с местонахождение на обекта в гр./с. {inputModel.ApiaryCity}, общ. {inputModel.ApiaryMunicipality}, обл. {inputModel.ApiaryState}. Регистриран ветеринарен лекар, " +
                                               $"обслужващ животновъдния обект, съгласно чл. 132, ал.1, т.1 от Закона за ветеринарномедицинската дейност {inputModel.VetNames}";

            n += 3; // n=28
            ws.Cells[$"A{n}:I{n}"].Merge = true;
            ws.Cells[$"A{n}:I{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"A{n}:E{n}"].Style.Font.Bold = true;
            ws.Cells[$"A{n}:E{n}"].Value = "ДЕКЛАРИРАМ, ЧЕ:";

            n += 2; // n=30
            ws.Cells[$"A{n}:I{n + 1}"].Merge = true;
            ws.Cells[$"A{n}:I{n + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[$"A{n}:I{n + 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            ws.Cells[$"A{n}:I{n + 1}"].Style.WrapText = true;
            ws.Cells[$"A{n}:I{n + 1}"].Value = "1.\t\tживотните изброените в приложение № 1 /опис на животните в животновъдния обект \nса собственост на " + EmptyFieldMegaLong;

            n += 2; // n=32
            ws.Cells[$"A{n}:I{n + 1}"].Merge = true;
            ws.Cells[$"A{n}:I{n + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[$"A{n}:I{n + 1}"].Style.WrapText = true;
            ws.Cells[$"A{n}:I{n + 1}"].Value = "2.\t\tсъм запознат с изискванията на чл. 132, ал. 1, т. 4-7, 10, 19 и 21 от Закона за \nветеринарномедицинската дейност, съгласно които следва";

            n += 2; // n=35
            ws.Cells[$"A{n}:I{n + 18}"].Merge = true;
            ws.Cells[$"A{n}:I{n + 18}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[$"A{n}:I{n + 18}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            ws.Cells[$"A{n}:I{n + 18}"].Style.WrapText = true;
            ws.Cells[$"A{n}:I{n + 18}"].Style.Indent = 4;
            ws.Cells[$"A{n}:I{n + 18}"].Value = "- незабавно писмено да уведомя за умрели животни ветеринарния лекар, обслужващ " +
                                                "животновъдния ми обект и кмета на населеното място, и обекта за обезвреждане на странични " +
                                                "животински продукти, обслужващ съответната територия; " +

                                                "\n- в срок до 24 часа преди транспортиране, промяна на собствеността или клане на животни от " +
                                                "видове, които подлежат на идентификация, предназначени за лична консумация, писмено да " +
                                                "уведомя ветеринарния лекар, обслужващ животновъдния ми обект; " +

                                                "\n- в тридневен срок от раждането на животни от видове, които подлежат на идентификация, " +
                                                "писмено да уведомя ветеринарния лекар, обслужващ животновъдния ми обект да извърши " +
                                                "официална идентификация на новородените животни и да въведе данните от " +
                                                "идентификацията в Интегрираната информационна система на БАБХ, или за извършената " +
                                                "официална идентификация на новородените животни като му предоставя данните от идентификацията за въвеждането им в Интегрираната информационна система на БАБХ; " +

                                                "\n- да отговарям за официалната идентификация на животните; " +

                                                "\n- да не допускам в обекта животни, които не са идентифицирани; " +

                                                "\n- ежегодно в периода от 1 до 20 октомври да извърша инвентаризация на " +
                                                "животните в обекта и да предам инвентаризационния опис на официалния ветеринарен лекар, отговорен за общината на територията на която ги отглеждам.";

            n += 21; // n=56
            ws.Cells[$"A{n}:I{n + 10}"].Merge = true;
            ws.Cells[$"A{n}:I{n + 10}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[$"A{n}:I{n + 10}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            ws.Cells[$"A{n}:I{n + 10}"].Style.WrapText = true;
            ws.Cells[$"A{n}:I{n + 10}"].Style.Indent = 4;
            ws.Cells[$"A{n}:I{n + 10}"].Value = "3.\t\tми е известно, че при неизпълнение на тези мои задължения нося административна " +
                                               "отговорност по чл. 416 и чл. 417 от Закона за ветеринарномедицинската дейност." +

                                               "\n4.\t\tсъм определил предназначението на животните(крави и юници) за месо или мляко, " +
                                               "съобразно направлението, за което ги отглеждам." +

                                               "\n5.\t\tми е известно, че при подаване на невярна информация нося наказателна отговорност по чл. " +
                                               "313 от НК." +

                                               "\n6.\t\tПредоставям личните си данни доброволно и давам съгласието си Българската агенция по " +
                                               "безопасност на храните да ги обработва, съхранява и използва за изпълнение на законните " +
                                               "интереси на Агенцията и при спазване разпоредбите на Регламент(ЕС) 2016 / 679 относно " +
                                               "защитата на физическите лица във връзка с обработването на лични данни и относно " +
                                               "свободното движение на такива данни.";

            n += 12; // n=58
            ws.Cells[$"A{n}:E{n}"].Merge = true;
            ws.Cells[$"A{n}:E{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[$"A{n}:E{n}"].Style.Font.Size = 12;
            ws.Cells[$"A{n}:E{n}"].Value = "Дата на подаване на декларацията: ....................";

            ws.Cells[$"F{n}:I{n}"].Merge = true;
            ws.Cells[$"F{n}:I{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws.Cells[$"A{n}:E{n}"].Style.Font.Size = 12;
            ws.Cells[$"F{n}:I{n}"].Value = "Собственик на животните: ....................";


            n += 38; // n=96
            this.PrintTableHeader(ref ws, ref n);

            n += 5;

            for (int i = 1; i <= beehivesCount; i++)
            {
                n++;

                ws.Row(n).Height = 11;

                ws.Cells[$"A{n}:I{n}"].Style.Font.Size = 9;
                ws.Cells[$"A{n}:I{n}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[$"A{n}:I{n}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[$"A{n}:I{n}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[$"A{n}:I{n}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[$"A{n}:I{n}"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                ws.Cells[$"A{n}"].Value = $"{i}.";
                ws.Cells[$"A{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                ws.Cells[$"B{n}:D{n}"].Merge = true;
                ws.Cells[$"B{n}:D{n}"].Value = "ПЧЕЛНО СЕМЕЙСТВО";

                ws.Cells[$"F{n}:G{n}"].Merge = true;
                ws.Cells[$"F{n}:G{n}"].Value = $"{apiaryNumber} - {i}";

                ws.Cells[$"H{n}:I{n}"].Merge = true;

                if (i == beehivesCount)
                {
                    n++;
                    this.PrintTableFooter(ref ws, ref n);
                }

                if (i % 50 == 0)
                {
                    n++;
                    this.PrintTableFooter(ref ws, ref n);

                    if (beehivesCount > 50)
                    {
                        n++;
                        this.PrintTableHeader(ref ws, ref n);
                        n += 5;
                    }
                }
            }

            ws.Cells.Style.Font.Name = "Times New Roman";

            return pck;
        }

        private void PrintTableHeader(ref ExcelWorksheet ws, ref int n)
        {
            ws.Cells[$"G{n}:I{n}"].Merge = true;
            ws.Cells[$"G{n}:I{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws.Cells[$"G{n}:I{n}"].Style.Font.UnderLine = true;
            ws.Cells[$"G{n}:I{n}"].Value = "Приложение № 1 към т. 1";

            n += 2; // n=98
            ws.Cells[$"A{n}:I{n}"].Merge = true;
            ws.Cells[$"A{n}:I{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"A{n}:I{n}"].Style.Font.Bold = true;
            ws.Cells[$"A{n}:I{n}"].Style.Font.Size = 10;
            ws.Cells[$"A{n}:I{n}"].Value = "О П И С  НА  Ж И В О Т Н И Т Е  В  Ж И В О Т Н О В Ъ Д Н И Я  О Б Е К Т ";

            n += 2; // n=98
            ws.Cells[$"A{n}:A{n + 5}"].Merge = true;
            ws.Cells[$"A{n}:A{n + 5}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"A{n}:A{n + 5}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[$"A{n}:A{n + 5}"].Style.Font.Size = 11;
            ws.Cells[$"A{n}:A{n + 5}"].Style.WrapText = true;
            ws.Cells[$"A{n}:A{n + 5}"].Style.Font.Bold = true;
            ws.Cells[$"A{n}:A{n + 5}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"A{n}:A{n + 5}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"A{n}:A{n + 5}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"A{n}:A{n + 5}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"A{n}:A{n + 5}"].Value = "№ \nпо \nред";

            ws.Cells[$"B{n}:D{n + 5}"].Merge = true;
            ws.Cells[$"B{n}:D{n + 5}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"B{n}:D{n + 5}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[$"B{n}:D{n + 5}"].Style.Font.Size = 11;
            ws.Cells[$"B{n}:D{n + 5}"].Style.WrapText = true;
            ws.Cells[$"B{n}:D{n + 5}"].Style.Font.Bold = true;
            ws.Cells[$"B{n}:D{n + 5}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"B{n}:D{n + 5}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"B{n}:D{n + 5}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"B{n}:D{n + 5}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"B{n}:D{n + 5}"].Value = "Вид на животното";

            ws.Cells[$"E{n}:E{n + 5}"].Merge = true;
            ws.Cells[$"E{n}:E{n + 5}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"E{n}:E{n + 5}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[$"E{n}:E{n + 5}"].Style.Font.Size = 11;
            ws.Cells[$"E{n}:E{n + 5}"].Style.WrapText = true;
            ws.Cells[$"E{n}:E{n + 5}"].Style.Font.Bold = true;
            ws.Cells[$"E{n}:E{n + 5}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"E{n}:E{n + 5}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"E{n}:E{n + 5}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"E{n}:E{n + 5}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"E{n}:E{n + 5}"].Value = "Пол \nм / ж";

            ws.Cells[$"F{n}:G{n + 5}"].Merge = true;
            ws.Cells[$"F{n}:G{n + 5}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"F{n}:G{n + 5}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[$"F{n}:G{n + 5}"].Style.Font.Size = 11;
            ws.Cells[$"F{n}:G{n + 5}"].Style.WrapText = true;
            ws.Cells[$"F{n}:G{n + 5}"].Style.Font.Bold = true;
            ws.Cells[$"F{n}:G{n + 5}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"F{n}:G{n + 5}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"F{n}:G{n + 5}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"F{n}:G{n + 5}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"F{n}:G{n + 5}"].Value = "№ на средството за идентификация";

            ws.Cells[$"H{n}:I{n + 5}"].Merge = true;
            ws.Cells[$"H{n}:I{n + 5}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[$"H{n}:I{n + 5}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[$"H{n}:I{n + 5}"].Style.Font.Size = 11;
            ws.Cells[$"H{n}:I{n + 5}"].Style.WrapText = true;
            ws.Cells[$"H{n}:I{n + 5}"].Style.Font.Bold = true;
            ws.Cells[$"H{n}:I{n + 5}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"H{n}:I{n + 5}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"H{n}:I{n + 5}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"H{n}:I{n + 5}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"H{n}:I{n + 5}"].Value = "*Предназначение \nна \nкрави и юници \nза \nМесо / Мляко";
        }

        private void PrintTableFooter(ref ExcelWorksheet ws, ref int n)
        {
            ws.Cells[$"A{n}:E{n}"].Merge = true;
            ws.Cells[$"A{n}:E{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[$"A{n}:E{n}"].Style.Font.Size = 12;
            ws.Cells[$"A{n}:E{n}"].Value = "Дата на описа ....................";

            n++;
            ws.Cells[$"A{n}:E{n}"].Merge = true;
            ws.Cells[$"A{n}:E{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[$"A{n}:E{n}"].Style.Font.Size = 12;
            ws.Cells[$"A{n}:E{n}"].Value = "Дата на подаване на декларацията: ....................";

            ws.Cells[$"F{n}:I{n}"].Merge = true;
            ws.Cells[$"F{n}:I{n}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws.Cells[$"A{n}:E{n}"].Style.Font.Size = 12;
            ws.Cells[$"F{n}:I{n}"].Value = "Собственик на животните: ....................";
        }
    }
}
