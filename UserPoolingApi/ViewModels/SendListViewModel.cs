using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class SendListViewModel
    {
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string ReceiverName { get; set; }
        [Required]
        public string ReceiverEmail { get; set; }
        [Required]
        public string SendList { get; set; } //string like, "254, 258, 261"
        
    }
}
