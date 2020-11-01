namespace CourseWorksHandler.WEB.Models
{
    public class StudentInfo
    {
        public Student Student { get; set; }

        public AcademicGroup Group { get; set; }

        public Teacher Teacher { get; set; }

        public CourseWork CourseWork { get; set; }
    }
}
