using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UserPoolingApi.Migrations
{
    public partial class addedVirtualPositionDesired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_Users_PositionDesiredId",
                table: "Users",
                column: "PositionDesiredId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_PositionDesired_PositionDesiredId",
                table: "Users",
                column: "PositionDesiredId",
                principalTable: "PositionDesired",
                principalColumn: "PositionDesiredId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_PositionDesired_PositionDesiredId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PositionDesiredId",
                table: "Users");

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
    }
}
