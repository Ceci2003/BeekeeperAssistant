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
        public const string FarmNumberRequiredErrorMessage = "Въведете номер животновъден обект";
        public const string ApiaryExistsErrorMessage = "Вече съществува пчелин с въведения номер";
        public const string ApiaryTypeRequiredErrorMessage = "Въведете вид на пчелина";

        // Pagination values
        public const int ApiariesPerPage = 5;
        public const int BeehivesPerPage = 25;
        public const int QueensPerPage = 25;
    }
}
