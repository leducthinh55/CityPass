using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class updateuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Uid",
                table: "UserPass",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserUid1",
                table: "UserPass",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "UserUid1",
                table: "UserPass");

            migrationBuilder.AlterColumn<string>(
                name: "Uid",
                table: "UserPass",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserUid",
                table: "UserPass",
                type: "nvarchar(450)",
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
    }
}
