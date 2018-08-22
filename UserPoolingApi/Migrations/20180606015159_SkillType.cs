using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UserPoolingApi.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkillTypeId",
                table: "Skills",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SkillType",
                columns: table => new
                {
                    SkillTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SkillTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillType", x => x.SkillTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SkillTypeId",
                table: "Skills",
                column: "SkillTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_SkillType_SkillTypeId",
                table: "Skills",
                column: "SkillTypeId",
                principalTable: "SkillType",
                principalColumn: "SkillTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_SkillType_SkillTypeId",
                table: "Skills");

            migrationBuilder.DropTable(
                name: "SkillType");

            migrationBuilder.DropIndex(
                name: "IX_Skills_SkillTypeId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "SkillTypeId",
                table: "Skills");
        }
    }
}
