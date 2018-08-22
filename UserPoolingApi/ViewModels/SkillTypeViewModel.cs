using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class SkillTypeViewModel
    {
        public int SkillTypeId { get; set; }
        [Required]
        public string SkillTypeName { get; set; }
    }
}