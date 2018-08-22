using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string QuestionStr { get; set; }
        [ForeignKey("Test")]
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
        public int CorrectAnswer { get; set; }
        public ICollection<Choice> Choices { get; set; }

    }
}
