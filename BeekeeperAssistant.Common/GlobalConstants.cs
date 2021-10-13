namespace BeekeeperAssistant.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "BeekeeperAssistant";

        public const string AdministratorRoleName = "Administrator";

        // Pagination values
        public const int ApiariesPerPage = 5;
        public const int ApiaryHelpersApiaryPerPage = 5;
        public const int BeehivesPerPage = 25;
        public const int QueensPerPage = 15;

        public const string ConfirmEmailHtml = "<tr style=\" display: flex; align-items: center; justify-content: center; font-family: Helvetica, Arial, sans-serif; height: 200px; \" >" +
                                                    "<a href = \" https://localhost:44302 \" style=\"display: flex; flex-direction: column; align-items: center; justify-content: center; text-decoration: none; color: #110400;\">" +
                                                        "<img src = \" https://localhost:44302/img/BA_demoLogo.png \" alt=\"BeekeeperAssistant\" >" +
                                                        "<h1>BeekeeperAssistant</h1>" +
                                                    "</a>" +
                                                "</tr>" +
                                                "<tr style = \"display: flex; flex-direction: column; align-items: center; justify-content: center; background-color: #110400; color: #ffffff; font-family: Helvetica, Arial, sans-serif; height: 250px;\" > " +
                                                    "<img src = \" https://localhost:44302/img/check_white.png \" alt= \"check \" style= \"margin: 0; height: 50px; background: #ff7800; border-radius: 30px;\">" +
                                                    "<h2 style = \"margin: 0; margin: 30px 0;\" > Регистрацията е успешна!</h2>" +
                                                    "<h3 style = \"margin: 0;\" > Натиснете бутона по-долу, за да потвърдите имейла си.</h3>" +
                                                "</tr>"
                                            ;

        // Apiary Constants
        public const string ApiaryPartNumberRegex = @"\d{4}";
        public const string ApiaryFullNumberRegex = @"\b([\d]{4}\b)-(\b\d{4})\b";
        public const int MaxPartNumberLength = 4;
        public const string CityCodeRequiredErrorMessage = "Въведете номер на населеното място";
        public const string AddressRequiredErrorMessage = "Въведете населеното място";
        public const string FarmNumberRequiredErrorMessage = "Въведете номер животновъден обект";
        public const string ApiaryExistsErrorMessage = "Вече съществува пчелин с въведения номер";
        public const string ApiaryTypeRequiredErrorMessage = "Въведете вид на пчелина";
        public static readonly string[] ApiaryTypeChartColors = new string[] { "'#ffffd4'", "'#fee391'", "'#fec44f'", "'#fe9929'", "'#ec7014'", "'#cc4c02'", "'#8c2d04'" };
        public static readonly string[] BeehivePowerChartColors = new string[] { "'#1a5918'", "'#fead34'", "'#8c0712'" };
        public static readonly string[] QueenYearChartColors = new string[] { "'#0A0ABF'", "'#FFFFFF'", "'#FFCD00'", "'#BF0A0A'", "'#007F0E'" };
    }
}
