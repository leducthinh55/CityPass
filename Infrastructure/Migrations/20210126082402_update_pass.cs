using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class update_pass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_UserPass_UserPassId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "ChildrenPackage",
                table: "Pass");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserPassId",
                table: "Ticket",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_UserPass_UserPassId",
                table: "Ticket",
                column: "UserPassId",
                principalTable: "UserPass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_UserPass_UserPassId",
                table: "Ticket");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserPassId",
                table: "Ticket",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "ChildrenPackage",
                table: "Pass",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_UserPass_UserPassId",
                table: "Ticket",
                column: "UserPassId",
                principalTable: "UserPass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
