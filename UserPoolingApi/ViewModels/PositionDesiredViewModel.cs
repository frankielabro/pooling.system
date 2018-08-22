using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class PositionDesiredViewModel
    {
        [Required]
        public int PositionDesiredId { get; set; }
        [Required]
        public string PositionName { get; set; }

    }
}
