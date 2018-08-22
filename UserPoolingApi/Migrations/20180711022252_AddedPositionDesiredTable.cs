using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UserPoolingApi.Migrations
{
    public partial class AddedPositionDesiredTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Position",
                table: "Users",
                newName: "PositionDesiredId");

            migrationBuilder.CreateTable(
                name: "PositionDesired",
                columns: table => new
                {
                    PositionDesiredId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PositionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionDesired", x => x.PositionDesiredId);
                });

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

            migrationBuilder.DropTable(
                name: "PositionDesired");

            migrationBuilder.DropIndex(
                name: "IX_Users_PositionDesiredId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "PositionDesiredId",
                table: "Users",
                newName: "Position");
        }
    }
}
