using System.ComponentModel;

namespace VideoOnDemand.Modules.DTOModels
{
    public class UserCourseDTO
    {
        [DisplayName("User Id")]
        public string UserId { get; set; }

        [DisplayName("Course Id")]
        public string CourseId { get; set; }

        [DisplayName("Email")]
        public string UserEmail { get; set; }

        [DisplayName("Title")]
        public string CourseTitle { get; set; }
    }
}
