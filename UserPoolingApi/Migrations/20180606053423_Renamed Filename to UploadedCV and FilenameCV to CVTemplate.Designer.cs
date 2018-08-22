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
    [Migration("20180606053423_Renamed Filename to UploadedCV and FilenameCV to CVTemplate")]
    partial class RenamedFilenametoUploadedCVandFilenameCVtoCVTemplate
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

                    b.ToTable("SkillType");
                });

            modelBuilder.Entity("UserPoolingApi.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("CVTemplate");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Phone");

                    b.Property<int>("Status");

                    b.Property<DateTime>("StatusDate");

                    b.Property<DateTime>("SubmittedDate");

                    b.Property<string>("UploadedCV");

                    b.HasKey("UserId");

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

            modelBuilder.Entity("UserPoolingApi.Models.Skill", b =>
                {
                    b.HasOne("UserPoolingApi.Models.SkillType", "SkillType")
                        .WithMany()
                        .HasForeignKey("SkillTypeId")
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
#pragma warning restore 612, 618
        }
    }
}
