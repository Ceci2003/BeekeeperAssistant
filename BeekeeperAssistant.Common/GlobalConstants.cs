namespace BeekeeperAssistant.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "BeekeeperAssistant";
        public const string SystemVersion = "v1.3.2";
        public const string AdministratorRoleName = "Administrator";
        public const string SearchKeywords = "BeekeeperAssistant, пчеларство, пчели, дфз, фонд земеделие, държавен фонд земеделие, пчеларски софтуер, пчеларска програма, пчеларски програми, земеделие";
        public const string SearchDescription = "BeekeeperAssistant е съвременният дневник на пчеларя. Той дава на пчеларите възможност да улеснят работата си, като следят информацията и дейностите по пчелините и кошерите.";
        public const string RequiredMessage = "Полетата с * са задължителни!";
        public const string RequiredMessageWithParam = "Полето {0} е задължителнo!";

        // Creation status messages
        public const string SuccessMessage = "SuccessMessage";
        public const string ErrorMessage = "ErrorMessage";

        // Pagination values
        public const int ApiariesPerPage = 5;
        public const int ApiaryHelpersApiaryPerPage = 5;
        public const int BeehivesPerPage = 25;
        public const int QueensPerPage = 25;
        public const int ApiariesPerPageAdministration = 25;
        public const int ApiaryHelpersPerPageAdministration = 25;
        public const int BeehivesPerPageAdministration = 25;
        public const int UsersPerPageAdministration = 25;

        // Apiary Constants
        public const string ApiaryPartNumberRegex = @"\d{4}";
        public const string ApiaryFullNumberRegex = @"\b([\d]{4}\b)-(\b\d{4})\b";
        public const int MaxPartNumberLength = 4;
        public const string CityCodeRequiredErrorMessage = "Въведете номер на населеното място";
        public const string AddressRequiredErrorMessage = "Въведете населеното място";
        public const string FarmNumberRequiredErrorMessage = "Въведете номер животновъден обект";
        public const string ApiaryExistsErrorMessage = "Вече съществува пчелин с въведения номер";
        public const string ApiaryTypeRequiredErrorMessage = "Въведете вид на пчелина";

        // Apiary and beehive constants
        public static readonly string[] NoteBackgroundColors = new string[] { "#F36921", "#FFC444", "#F9EDA1", "#ACD369", "#96D9EC" };

        // Quickchart colors
        public static readonly string[] ApiaryTypeChartColors = new string[] { "'#ffffd4'", "'#fee391'", "'#fec44f'", "'#fe9929'", "'#ec7014'", "'#cc4c02'", "'#8c2d04'" };
        public static readonly string[] BeehivePowerChartColors = new string[] { "'#59A14F'", "'#fead34'", "'#DA3C3D'" };
        public static readonly string[] QueenYearChartColors = new string[] { "'#689dd6'", "'#FFFFFF'", "'#FFCD00'", "'#BF0A0A'", "'#007F0E'" };
    }
}
