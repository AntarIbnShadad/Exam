namespace Exam.Models.ViewModel
{
    public class ApproveEnrollmentViewModel
    {
        public int Id { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public string InstructorName { get; set; }
        public string StrudentName { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
