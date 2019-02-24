using System.Collections.Generic;
using VideoOnDemand.Modules.DTOModels;

namespace VideoOnDemand.Modules.MembershipViewModels
{
    public class DashboardViewModel
    {
        public List<List<CourseDTO>> Courses { get; set; }
    }
}
