namespace SMSClient.Model
{
    public class Department
    {
        public int Id { get; set; } 
        public string ShortName { get; set; } = null!;
        public string LongName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;  

        public bool? IsActive { get; set; }
        public ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}
