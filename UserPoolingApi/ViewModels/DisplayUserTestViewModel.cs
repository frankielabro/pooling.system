using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class DisplayUserTestViewModel
    {
        public int UserTestId { get; set; }
        public int TestId { get; set; }
        public int Score { get; set; }
        public string TestType { get; set; }
        public DateTime DateTaken { get; set; }
        public DisplayUserForTestResultViewModel User { get; set; }

    }
}
