using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class UserTestViewModel
    {
        public int UserTestId { get; set; }
        public int Score { get; set; }
        public DateTime DateTaken { get; set; }
        public DisplayTestForTestResultViewModel Test { get; set; }
    }
}
