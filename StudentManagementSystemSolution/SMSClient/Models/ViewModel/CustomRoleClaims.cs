using SMSClient.Authentication;

namespace SMSClient.Models.ViewModel
{
    public class CustomRoleClaims
    {
        public string DisplayClaimName { get; set; }
        public string AccessLevel { get; set; }
        public string Module { get; set; }

        public string Value { get { return Permission.Value(AccessLevel, Module); } }

    }
}
