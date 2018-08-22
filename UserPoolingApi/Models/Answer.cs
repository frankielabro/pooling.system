using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        [ForeignKey("Questions")]
        public int QuestionId { get; set; }
        public virtual Question Questions { get; set; }
        public int ChoiceId { get; set; }
        public string AnswerStr { get; set; }

    }
}
