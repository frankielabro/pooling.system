using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UserPoolingApi.Migrations
{
    public partial class removedHourlyRateinSurvey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HourlyRate",
                table: "Survey");

            migrationBuilder.AlterColumn<string>(
                name: "NoticePeriod",
                table: "Survey",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "MonthlyRate",
                table: "Survey",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NoticePeriod",
                table: "Survey",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MonthlyRate",
                table: "Survey",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HourlyRate",
                table: "Survey",
                nullable: false,
                defaultValue: 0);
        }
    }
}
