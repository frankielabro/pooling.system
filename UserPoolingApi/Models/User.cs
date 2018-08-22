using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using UserPoolingApi.Enums;

namespace UserPoolingApi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string UploadedCV { get; set; }
        public string CVTemplate { get; set; }
        public StatusEnum Status { get; set; }
        [ForeignKey("PositionDesired")]
        public int PositionDesiredId { get; set; }
        public DateTime StatusDate { get; set; }
        public DateTime SubmittedDate { get; set; }
        public int IsActive { get; set; }
        public string Availability { get; set; }
        public int IsOutsider { get; set; }
        public ICollection<UserSkills> UserSkills { get; set; }
        public ICollection<CustomSkill> CustomSkills { get; set; }
        public ICollection<WorkExperience> WorkExperiences { get; set; }
        public ICollection<Summary> Summaries { get; set; }
        public ICollection<Education> Educations { get; set; }
        public ICollection<Certification> Certificates { get; set; }
        //it is important to add "virtual" so that foreign key constraint will be accepted.
        public virtual PositionDesired PositionDesired { get; set; }
        public ICollection<Survey> Survey { get; set; }
        public ICollection<UserTest> UserTest { get; set; }
    }
}
