﻿using System.ComponentModel.DataAnnotations;

namespace CourseWorksHandler.WEB.ViewModels
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password), MinLength(1)]
        public string Password { get; set; }
    }

}
