namespace CourseWorksHandler.WEB.Models
{
    public class Teacher
    {
        public int Id { set; get; }

        public string FullName { set; get; }
    }

    public class AppUser
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string PasswordHashed { get; set; }

        public string RoleName { get; set; }
    }
}
