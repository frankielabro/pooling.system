﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserPoolingApi.ViewModels
{
    public class DisplayUserViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Availability { get; set; }
        public string UploadedCV { get; set; }
        public string CVTemplate { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime StatusDate { get; set; }
        public int PositionDesiredId { get; set; }
        public string PositionName { get; set; }
        public int MyProperty { get; set; }
        public int IsOutsider { get; set; }
        public IEnumerable<DisplayUserSkillViewModel> UserSkills { get; set; }
        public IEnumerable<DisplaySurveyViewModel> Survey { get; set; }
    }
}
