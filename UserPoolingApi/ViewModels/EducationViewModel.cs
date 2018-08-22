using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class EducationViewModel
    {
        public int EducationId { get; set; }
        public string SchoolName { get; set; }
        public string Course { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
