using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoOnDemand.Entities
{
    public class Video
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(80)]
        public string Title { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        [MaxLength(1024)]
        public string Thumbnail { get; set; }

        [MaxLength(1024)]
        public string Url { get; set; }

        public int Duration { get; set; }
        public int Position { get; set; }
        public int ModuleId { get; set; }
        public int CourseId { get; set; }
    }
}
