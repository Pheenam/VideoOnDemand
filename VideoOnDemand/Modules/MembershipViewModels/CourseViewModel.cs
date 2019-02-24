using System.Collections.Generic;
using VideoOnDemand.Modules.DTOModels;


namespace VideoOnDemand.Modules.MembershipViewModels
{
    public class CourseViewModel
    {
        public CourseDTO Course { get; set; }
        public InstructorDTO Instructor { get; set; }
        public IEnumerable<ModuleDTO> Modules { get; set; }
    }
}
