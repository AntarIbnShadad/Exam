namespace Exam.Models.ViewModel
{
    public class StudentScheduleViewModel
    {
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public string InstructorName { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsActive { get; set; }
    }
}
