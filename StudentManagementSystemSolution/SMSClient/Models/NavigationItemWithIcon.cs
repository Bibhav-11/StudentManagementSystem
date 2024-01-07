using System;

namespace SMSClient.Models
{
    public class NavigationItemWithIcon
    {
        public int id { get; set; }
        public string text { get; set; }
        public string icon { get; set; }

        public string Controller { get; set; }  
        public string Action { get; set; }  

        public IEnumerable<NavigationItemWithIcon> Items { get; set; }
    }
}
