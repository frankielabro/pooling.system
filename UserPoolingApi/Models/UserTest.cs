using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.Models
{
    public class UserTest
    {
        public int UserTestId { get; set; }
        [ForeignKey("Test")]
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int IsTaken { get; set; }
        public int IsSubmit { get; set; }
        public int Score { get; set; }
        public DateTime DateTaken { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
