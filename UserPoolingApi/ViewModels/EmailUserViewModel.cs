using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class EmailUserViewModel
    {
        [Required]
        public string UserIds { get; set; }
        [Required]
        public int MessageId { get; set; }
        [Required]
        public string CompanyName { get; set; }
    }
}

