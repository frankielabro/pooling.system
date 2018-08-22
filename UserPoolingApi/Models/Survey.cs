using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.Models
{
    public class Survey
    {
        public int SurveyId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string ReachedBy { get; set; }
        public string PreferredWorkplace { get; set; }
        public string MonthlyRate { get; set; }
        public string NoticePeriod { get; set; }
    }
}
