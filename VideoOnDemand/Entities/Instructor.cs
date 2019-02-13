using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace VideoOnDemand.Entities
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(1024), Required]
        public string Name { get; set; }
        
        [MaxLength(1024)]
        public string Description { get; set; }

        [MaxLength(1024)]
        public string Thumbnail { get; set; }

        public List<Course> Courses { get; set; }
    }
}
