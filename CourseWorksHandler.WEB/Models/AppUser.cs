namespace CourseWorksHandler.WEB.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string PasswordHashed { get; set; }

        public string RoleName { get; set; }
    }
}
