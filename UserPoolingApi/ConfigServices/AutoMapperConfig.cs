using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UserPoolingApi.Enums;
using UserPoolingApi.Helper;
using UserPoolingApi.Models;
using UserPoolingApi.Models.Enums;
using UserPoolingApi.ViewModels;

namespace UserPoolingApi.ConfigServices
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            /* ReverseMap is used to map from one object to another object vice-versa
             */
            CreateMap<User, UserViewModel>().ReverseMap()
            .ForMember(a => a.Status, b => b.ResolveUsing(a => StatusEnum.New));
            CreateMap<User, UserViewModel2>().ReverseMap()
            .ForMember(a => a.UploadedCV, b => b.ResolveUsing(a => a.UploadedCV.FileName));
            CreateMap<WorkExperience, WorkExperienceViewModel>().ReverseMap();
            CreateMap<WorkExperience, PostWorkExperienceViewModel>().ReverseMap();
            CreateMap<Summary, SummaryViewModel>().ReverseMap();
            CreateMap<Education, EducationViewModel>().ReverseMap();
            CreateMap<Education, PostEducationViewModel>().ReverseMap();
            CreateMap<Certification, CertificationViewModel>().ReverseMap();

            /* Automapper is used to easily copy the values from Source object to the Destination object.
               An example below transfers User to DisplayUserViewModel
               At the second line, "a" represents User while "b" represents DisplayUserModel.
               User.Status is an enum type(sort of array) while DisplayUSerViewModel.Status is a string type
               That's why it needs to be resolved.             
             */

            CreateMap<User, DisplayUserViewModel>()
                .ForMember(a => a.Status, b => b.ResolveUsing(a => a.Status.ToString().ToSentenceCase()))
                .ForMember(a => a.PositionName, b => b.ResolveUsing(a => a.PositionDesired.PositionName));
            CreateMap<Skill, SkillViewModel>().ReverseMap();
            CreateMap<Skill, UpdateSkillTypeViewModel>().ReverseMap();
            CreateMap<SkillType, SkillTypeViewModel>().ReverseMap();
            CreateMap<UserSkillViewModel, UserSkills>().ReverseMap();
            CreateMap<PostUserSkillViewModel, UserSkills>().ReverseMap();
            CreateMap<DisplayUserSkillViewModel, UserSkills>().ReverseMap()
                .ForMember(a => a.SkillName, b => b.ResolveUsing(a => a.Skill.SkillName));
            CreateMap<User, PageResultViewModel>().ReverseMap();
            CreateMap<Message, MessageTemplateCreateViewModel>().ReverseMap();
            CreateMap<PositionDesired, PositionDesiredViewModel>().ReverseMap();
            CreateMap<CustomSkillViewModel, CustomSkill>().ReverseMap();
            CreateMap<User, UpdateUserViewModel>().ReverseMap();
            CreateMap<Survey, SurveyViewModel>().ReverseMap();
            CreateMap<Survey, DisplaySurveyViewModel>().ReverseMap();
            CreateMap<UserSkills, EditUserSkillViewModel>().ReverseMap();
            CreateMap<Test, TestViewModel>().ReverseMap();
            CreateMap<UserTest, SubmitAnswerViewModel>().ReverseMap();
            CreateMap<Answer, UserAnswersViewModel>().ReverseMap();
            CreateMap<User, GetTestResultViewModel>().ReverseMap();
            CreateMap<UserTest, DisplayUserTestViewModel>().ReverseMap();
            CreateMap<TestType, TestTypeViewModel>().ReverseMap();
            CreateMap<User, DisplayUserForTestResultViewModel>().ReverseMap();
            CreateMap<Test, DisplayTestForTestResultViewModel>().ReverseMap();
            CreateMap<UserTest, GetTestViewModel>().ReverseMap();
            CreateMap<UserTest, UserTestViewModel>().ReverseMap();
            CreateMap<User, UserUserTestViewModel>().ReverseMap();

        }

        private void AfterMap(Func<object, object, object> p)
        {
            throw new NotImplementedException();
        }
    }

    public static class MapperConfigService
    {
        public static IServiceCollection RegisterMapper(this IServiceCollection services)
        {
            services.AddAutoMapper();

            return services;
        }
    }
}
