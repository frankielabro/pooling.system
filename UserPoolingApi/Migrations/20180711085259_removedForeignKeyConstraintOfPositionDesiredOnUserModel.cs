using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UserPoolingApi.Migrations
{
    public partial class removedForeignKeyConstraintOfPositionDesiredOnUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_PositionDesired_PositionDesiredId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PositionDesiredId",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
