using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.Models
{
    public class Education
    {
        public int EducationId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
        public string SchoolName { get; set; }
        public string Course { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        
    }
}
