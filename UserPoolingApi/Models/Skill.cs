using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.Models
{
    public class Skill
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }

        [ForeignKey("SkillType")]
        public int SkillTypeId { get; set; }

        public SkillType SkillType { get; set; }
    }
}
