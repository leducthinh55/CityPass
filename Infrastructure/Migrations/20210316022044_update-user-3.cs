using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class updateuser3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPass_User_Uid",
                table: "UserPass");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPass_User_UserUid1",
                table: "UserPass");

            migrationBuilder.DropIndex(
                name: "IX_UserPass_Uid",
                table: "UserPass");

            migrationBuilder.DropIndex(
                name: "IX_UserPass_UserUid1",
                table: "UserPass");

            migrationBuilder.DropColumn(
                name: "Uid",
                table: "UserPass");

            migrationBuilder.DropColumn(
                name: "UserUid1",
                table: "UserPass");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Uid",
                table: "UserPass",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserUid1",
                table: "UserPass",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPass_Uid",
                table: "UserPass",
                column: "Uid");

            migrationBuilder.CreateIndex(
                name: "IX_UserPass_UserUid1",
                table: "UserPass",
                column: "UserUid1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPass_User_Uid",
                table: "UserPass",
                column: "Uid",
                principalTable: "User",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPass_User_UserUid1",
                table: "UserPass",
                column: "UserUid1",
                principalTable: "User",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
