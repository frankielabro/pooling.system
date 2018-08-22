using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UserPoolingApi.Migrations
{
    public partial class changedpositionDesiredfromInttoICollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PositionDesired",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PositionDesired_UserId",
                table: "PositionDesired",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PositionDesired_Users_UserId",
                table: "PositionDesired",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PositionDesired_Users_UserId",
                table: "PositionDesired");

            migrationBuilder.DropIndex(
                name: "IX_PositionDesired_UserId",
                table: "PositionDesired");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PositionDesired");
        }
    }
}
