using System;

namespace CourseWorksHandler.WEB.Models
{
    public class CourseWork
    {
        public int Id { set; get; }

        public string Theme { set; get; }

        public string Task { set; get; }

        public DateTime SubmissionTime { set; get; }
    }
}
