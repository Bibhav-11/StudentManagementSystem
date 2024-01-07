namespace SMSClient.Authentication
{
    public static class Permission
    {
        public static string Value(string accessLevel, string module)
        {
            return $"{accessLevel}_{module}";
        }
    }
}
