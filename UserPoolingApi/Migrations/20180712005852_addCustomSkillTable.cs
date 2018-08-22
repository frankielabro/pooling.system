using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace UserPoolingApi.Migrations
{
    public partial class addCustomSkillTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomSkills",
                columns: table => new
                {
                    CustomSkillId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    SkillLevel = table.Column<string>(nullable: true),
                    SkillName = table.Column<string>(nullable: true),
                    SkillTypeId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    YearsOfExperience = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomSkills", x => x.CustomSkillId);
                    table.ForeignKey(
                        name: "FK_CustomSkills_SkillTypes_SkillTypeId",
                        column: x => x.SkillTypeId,
                        principalTable: "SkillTypes",
                        principalColumn: "SkillTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomSkills_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DateAdded",
                table: "CustomSkills",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
