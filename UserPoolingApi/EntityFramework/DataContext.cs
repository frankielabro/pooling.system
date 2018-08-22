using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserPoolingApi.Models;

namespace UserPoolingApi.EntityFramework
{
    public class DataContext : DbContext
    {
        //DataContext is your database
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        //The following represent your tables in the database
        //public DbSet<__Model Name__> __LocalObjectName__ { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<UserSkills> UserSkills { get; set; }

        //The following are the added tables by me for the Pooling System
        public DbSet<PositionDesired> PositionDesired { get; set; }
        public DbSet<SkillType> SkillTypes { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Summary> Summaries { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<CustomSkill> CustomSkills { get; set; }
        public DbSet<Survey> Survey { get; set; }

        //for the User Online Test
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Test> Test { get; set; }
        public DbSet<TestType> TestTypes { get; set; }
        public DbSet<UserTest> UserTests { get; set; }

    }
}
