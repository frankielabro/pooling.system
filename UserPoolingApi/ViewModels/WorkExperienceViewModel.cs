using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class WorkExperienceViewModel
    {
        public int WorkExperienceId { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string WorkDescription { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
