using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class TestViewModel
    {
        public int UserId { get; set; }
        public int emailStatus { get; set; }
        public int isTaken { get; set; }
        public IEnumerable<QuestionViewModel> Question { get; set; }
    }
}
