using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    /*This class is a component in creating a user*/
    public class UserViewModel
    {
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int PositionDesiredId { get; set; }
        [Required]
        public string Availability { get; set; }
        public IList<PostUserSkillViewModel> UserSkills { get; set; }
        public IList<PostWorkExperienceViewModel> WorkExperiences { get; set; }
        public IList<PostEducationViewModel> Educations { get; set; }
        public IList<SummaryViewModel> Summaries { get; set; }
        public IList<CertificationViewModel> Certificates { get; set; }
        public IList<CustomSkillViewModel> CustomSkills { get; set; }


    }
}
