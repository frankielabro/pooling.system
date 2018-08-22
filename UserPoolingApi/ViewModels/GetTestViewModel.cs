using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class GetTestViewModel
    {
        [Required]
        public string email { get; set; }
        [Required]
        public int testId{ get; set; }
    }
}
