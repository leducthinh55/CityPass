using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class updateuser4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserUid",
                table: "UserPass",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPass_UserUid",
                table: "UserPass",
                column: "UserUid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPass_User_UserUid",
                table: "UserPass",
                column: "UserUid",
                principalTable: "User",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPass_User_UserUid",
                table: "UserPass");

            migrationBuilder.DropIndex(
                name: "IX_UserPass_UserUid",
                table: "UserPass");

            migrationBuilder.DropColumn(
                name: "UserUid",
                table: "UserPass");
        }
    }
}
