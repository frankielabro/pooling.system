using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class SurveyViewModel
    {
        public int UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string ReachedBy { get; set; }
        [Required]
        public string PreferredWorkplace { get; set; }
        [Required]
        public string MonthlyRate { get; set; }
        [Required]
        public string NoticePeriod { get; set; }
    }
}
