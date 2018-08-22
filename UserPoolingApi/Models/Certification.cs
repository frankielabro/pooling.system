using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.Models
{
    public class Certification
    {
        public int CertificationId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        public string CompanyName { get; set; }
        public string Description { get; set; }
        public DateTime DateIssued { get; set; }
    }
}
