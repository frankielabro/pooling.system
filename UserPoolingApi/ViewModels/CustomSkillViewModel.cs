using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class CustomSkillViewModel
    {
       
        [Required]
        public string SkillName { get; set; }
        [Required]
        public string SkillLevel { get; set; }
        [Required]
        public int YearsOfExperience { get; set; }
        [Required]
        public int SkillTypeId { get; set; }

    }
}
