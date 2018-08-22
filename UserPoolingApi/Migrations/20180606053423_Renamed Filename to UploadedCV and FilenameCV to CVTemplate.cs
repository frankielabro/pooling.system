using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UserPoolingApi.Migrations
{
    public partial class RenamedFilenametoUploadedCVandFilenameCVtoCVTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilenameCV",
                table: "Users",
                newName: "UploadedCV");

            migrationBuilder.RenameColumn(
                name: "Filename",
                table: "Users",
                newName: "CVTemplate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UploadedCV",
                table: "Users",
                newName: "FilenameCV");

            migrationBuilder.RenameColumn(
                name: "CVTemplate",
                table: "Users",
                newName: "Filename");
        }
    }
}
