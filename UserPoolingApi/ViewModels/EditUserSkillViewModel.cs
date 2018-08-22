using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class EditUserSkillViewModel
    {
        public int UserId { get; set; }
        public int SkillId { get; set; }
        public string SkillLevel { get; set; }
        public int YearsOfExperience { get; set; }

    }
}
