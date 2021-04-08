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
        public const string CityCodeRequiredErrorMessage = "TODO: Implement";
        public const string FarmNumberRequiredErrorMessage = "TODO: Implement";
        public const string ApiaryExistsErrorMessage = "TODO: Implement";
        public const string ApiaryTypeRequiredErrorMessage = "TODO: Implement";
    }
}
