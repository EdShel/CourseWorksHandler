using System.ComponentModel.DataAnnotations;

namespace CourseWorksHandler.WEB.ViewModels
{
    public class RegisterTeacherModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Password), MinLength(1)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
