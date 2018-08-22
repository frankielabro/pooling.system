using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.Models
{
    public class Test
    {
        public int TestId { get; set; }
        [ForeignKey("TestType")]
        public int TestTypeId { get; set; }
        public virtual TestType TestType { get; set; }
        public ICollection<Question> Question { get; set; }
    }
}
