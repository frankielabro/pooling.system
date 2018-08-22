using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class GetTestResultViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int IsOutsider { get; set; }
        public IEnumerable<DisplayUserTestViewModel> UserTest { get; set; }
    }
}
