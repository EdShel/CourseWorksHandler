using System.ComponentModel.DataAnnotations;

namespace CourseWorksHandler.WEB.ViewModels
{
    public class RegisterStudentModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string GroupName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
