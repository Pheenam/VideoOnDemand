﻿using System.Collections.Generic;
using VideoOnDemand.Modules.DTOModels;

namespace VideoOnDemand.Modules.MembershipViewModels
{
    public class VideoViewModel
    {
        public VideoDTO Video { get; set; }
        public InstructorDTO Instructor { get; set; }
        public CourseDTO Course { get; set; }
        public LessonInfoDTO LessonInfo { get; set; }
    }
}
