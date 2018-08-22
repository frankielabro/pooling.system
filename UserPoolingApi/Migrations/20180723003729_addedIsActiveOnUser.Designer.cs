﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using UserPoolingApi.EntityFramework;
using UserPoolingApi.Enums;

namespace UserPoolingApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20180723003729_addedIsActiveOnUser")]
    partial class addedIsActiveOnUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("UserPoolingApi.Models.Admin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created_at");

                    b.Property<string>("Email");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<DateTime>("Updated_at");

                    b.Property<string>("Username");

                    b.HasKey("AdminId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("UserPoolingApi.Models.Certification", b =>
                {
                    b.Property<int>("CertificationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyName");

                    b.Property<DateTime>("DateIssued");

                    b.Property<string>("Description");

                    b.Property<int>("UserId");

                    b.HasKey("CertificationId");

                    b.HasIndex("UserId");

                    b.ToTable("Certifications");
                });

            modelBuilder.Entity("UserPoolingApi.Models.CustomSkill", b =>
                {
                    b.Property<int>("CustomSkillId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("SkillLevel");

                    b.Property<string>("SkillName");

                    b.Property<int>("SkillTypeId");

                    b.Property<int>("UserId");

                    b.Property<int>("YearsOfExperience");

                    b.HasKey("CustomSkillId");

                    b.HasIndex("SkillTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("CustomSkills");
                });

            modelBuilder.Entity("UserPoolingApi.Models.Education", b =>
                {
                    b.Property<int>("EducationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Course");

                    b.Property<DateTime>("FromDate");

                    b.Property<string>("SchoolName");

                    b.Property<DateTime>("ToDate");

                    b.Property<int>("UserId");

                    b.HasKey("EducationId");

                    b.HasIndex("UserId");

                    b.ToTable("Educations");
                });

            modelBuilder.Entity("UserPoolingApi.Models.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<int>("Position");

                    b.Property<string>("Subject");

                    b.HasKey("MessageId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("UserPoolingApi.Models.PositionDesired", b =>
                {
                    b.Property<int>("PositionDesiredId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PositionName");

                    b.HasKey("PositionDesiredId");

                    b.ToTable("PositionDesired");
                });

            modelBuilder.Entity("UserPoolingApi.Models.Skill", b =>
                {
                    b.Property<int>("SkillId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SkillName");

                    b.Property<int>("SkillTypeId");

                    b.HasKey("SkillId");

                    b.HasIndex("SkillTypeId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("UserPoolingApi.Models.SkillType", b =>
                {
                    b.Property<int>("SkillTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SkillTypeName");

                    b.HasKey("SkillTypeId");

                    b.ToTable("SkillTypes");
                });

            modelBuilder.Entity("UserPoolingApi.Models.Summary", b =>
                {
                    b.Property<int>("SummaryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Sentence");

                    b.Property<int>("UserId");

                    b.HasKey("SummaryId");

                    b.HasIndex("UserId");

                    b.ToTable("Summaries");
                });

            modelBuilder.Entity("UserPoolingApi.Models.Survey", b =>
                {
                    b.Property<int>("SurveyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MonthlyRate");

                    b.Property<string>("NoticePeriod");

                    b.Property<string>("PreferredWorkplace");

                    b.Property<string>("ReachedBy");

                    b.Property<int>("UserId");

                    b.HasKey("SurveyId");

                    b.HasIndex("UserId");

                    b.ToTable("Survey");
                });

            modelBuilder.Entity("UserPoolingApi.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("CVTemplate");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<int>("IsActive");

                    b.Property<string>("LastName");

                    b.Property<string>("Phone");

                    b.Property<int>("PositionDesiredId");

                    b.Property<int>("Status");

                    b.Property<DateTime>("StatusDate");

                    b.Property<DateTime>("SubmittedDate");

                    b.Property<string>("UploadedCV");

                    b.HasKey("UserId");

                    b.HasIndex("PositionDesiredId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserPoolingApi.Models.UserSkills", b =>
                {
                    b.Property<int>("UserSkillsId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SkillId");

                    b.Property<string>("SkillLevel");

                    b.Property<int>("UserId");

                    b.Property<int>("YearsOfExperience");

                    b.HasKey("UserSkillsId");

                    b.HasIndex("SkillId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSkills");
                });

            modelBuilder.Entity("UserPoolingApi.Models.WorkExperience", b =>
                {
                    b.Property<int>("WorkExperienceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyName");

                    b.Property<DateTime>("FromDate");

                    b.Property<string>("Position");

                    b.Property<DateTime>("ToDate");

                    b.Property<int>("UserId");

                    b.Property<string>("WorkDescription");

                    b.HasKey("WorkExperienceId");

                    b.HasIndex("UserId");

                    b.ToTable("WorkExperiences");
                });

            modelBuilder.Entity("UserPoolingApi.Models.Certification", b =>
                {
                    b.HasOne("UserPoolingApi.Models.User", "User")
                        .WithMany("Certificates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UserPoolingApi.Models.CustomSkill", b =>
                {
                    b.HasOne("UserPoolingApi.Models.SkillType", "SkillType")
                        .WithMany()
                        .HasForeignKey("SkillTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UserPoolingApi.Models.User", "User")
                        .WithMany("CustomSkills")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UserPoolingApi.Models.Education", b =>
                {
                    b.HasOne("UserPoolingApi.Models.User", "User")
                        .WithMany("Educations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UserPoolingApi.Models.Skill", b =>
                {
                    b.HasOne("UserPoolingApi.Models.SkillType", "SkillType")
                        .WithMany()
                        .HasForeignKey("SkillTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UserPoolingApi.Models.Summary", b =>
                {
                    b.HasOne("UserPoolingApi.Models.User", "User")
                        .WithMany("Summaries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UserPoolingApi.Models.Survey", b =>
                {
                    b.HasOne("UserPoolingApi.Models.User", "User")
                        .WithMany("Survey")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UserPoolingApi.Models.User", b =>
                {
                    b.HasOne("UserPoolingApi.Models.PositionDesired", "PositionDesired")
                        .WithMany()
                        .HasForeignKey("PositionDesiredId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UserPoolingApi.Models.UserSkills", b =>
                {
                    b.HasOne("UserPoolingApi.Models.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UserPoolingApi.Models.User", "User")
                        .WithMany("UserSkills")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UserPoolingApi.Models.WorkExperience", b =>
                {
                    b.HasOne("UserPoolingApi.Models.User", "User")
                        .WithMany("WorkExperiences")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
