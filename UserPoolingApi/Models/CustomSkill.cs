using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.Models
{
    public class CustomSkill
    {
        public int CustomSkillId { get; set; }
        public string SkillName { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        [ForeignKey("SkillType")]
        public int SkillTypeId { get; set; }

        public SkillType SkillType { get; set; }
        public DateTime DateAdded { get; set; }
        public string SkillLevel { get; set; }
        public int YearsOfExperience { get; set; }
        
    }
}
