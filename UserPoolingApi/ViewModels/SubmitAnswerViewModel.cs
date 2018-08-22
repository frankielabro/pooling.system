using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class SubmitAnswerViewModel
    {
        [Required]
        public int UserId { get; set; }
        public int TestId { get; set; }
        public IEnumerable<UserAnswersViewModel> Answers { get; set; }
    }
}
