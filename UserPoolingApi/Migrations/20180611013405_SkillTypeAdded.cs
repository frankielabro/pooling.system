using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UserPoolingApi.Migrations
{
    public partial class SkillTypeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_SkillType_SkillTypeId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SkillType",
                table: "SkillType");

            migrationBuilder.RenameTable(
                name: "SkillType",
                newName: "SkillTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SkillTypes",
                table: "SkillTypes",
                column: "SkillTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_SkillTypes_SkillTypeId",
                table: "Skills",
                column: "SkillTypeId",
                principalTable: "SkillTypes",
                principalColumn: "SkillTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_SkillTypes_SkillTypeId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SkillTypes",
                table: "SkillTypes");

            migrationBuilder.RenameTable(
                name: "SkillTypes",
                newName: "SkillType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SkillType",
                table: "SkillType",
                column: "SkillTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_SkillType_SkillTypeId",
                table: "Skills",
                column: "SkillTypeId",
                principalTable: "SkillType",
                principalColumn: "SkillTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
