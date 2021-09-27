namespace BeekeeperAssistant.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "BeekeeperAssistant";

        public const string AdministratorRoleName = "Administrator";

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

        //public static string[] ApiaryTypeChartColors = vs;
        //public static string[] BeehivePowerChartColors = vs;
        //public static string[] ApiaryChartColors = new string[] { "'#558b2f'", "'#8faa4a'", "'#c8c96b'", "'#ffe891'", "'#f7b560'", "'#ec7d44'", "'#da3c3d'" };

        // Pagination values
        public const int ApiariesPerPage = 10;
        public const int BeehivesPerPage = 25;
        public const int QueensPerPage = 15;
    }
}
