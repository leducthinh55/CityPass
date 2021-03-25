using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThemeColor",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlImage",
                table: "Category",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ThemeColor",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "UrlImage",
                table: "Category");
        }
    }
}
