using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionStr { get; set; }

        public IEnumerable<ChoiceViewModel> Choices { get; set; }
        //public int CorrectAnswer { get; set; }
    }
}
