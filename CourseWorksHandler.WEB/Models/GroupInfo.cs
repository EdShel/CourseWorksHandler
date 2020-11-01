namespace CourseWorksHandler.WEB.Models
{
    public sealed class GroupInfo
    {
        public int Id { get; set; }

        public string GroupName { get; set; }

        public int TeacherId { get; set; }

        public string TeacherName { get; set; }

        public int StudentsCount { get; set; }

        public int AverageMark { get; set; }
    }
}
