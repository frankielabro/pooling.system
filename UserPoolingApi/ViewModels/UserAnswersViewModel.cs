using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class UserAnswersViewModel
    {
        public int QuestionId { get; set; }
        public int ChoiceId { get; set; }
        public string AnswerStr { get; set; }
    }
}
