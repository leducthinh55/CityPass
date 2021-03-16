using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class editdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pass_Pass_PassChildrenId",
                table: "Pass");

            migrationBuilder.DropIndex(
                name: "IX_Pass_PassChildrenId",
                table: "Pass");

            migrationBuilder.DropColumn(
                name: "PassChildrenId",
                table: "Pass");

            migrationBuilder.AddColumn<bool>(
                name: "IsChildren",
                table: "UserPass",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChildren",
                table: "UserPass");

            migrationBuilder.AddColumn<Guid>(
                name: "PassChildrenId",
                table: "Pass",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pass_PassChildrenId",
                table: "Pass",
                column: "PassChildrenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pass_Pass_PassChildrenId",
                table: "Pass",
                column: "PassChildrenId",
                principalTable: "Pass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
