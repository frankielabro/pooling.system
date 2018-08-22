using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class MessageTemplateCreateViewModel
    {

        public int MessageId { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
        [Required]
        public int Position { get; set; }

    }
}
