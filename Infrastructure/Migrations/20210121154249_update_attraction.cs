using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class update_attraction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketType_Atrraction_AtrractionId",
                table: "TicketType");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingTime_Atrraction_AtrractionId",
                table: "WorkingTime");

            migrationBuilder.DropTable(
                name: "Atrraction");

            migrationBuilder.CreateTable(
                name: "Attraction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    IsTemporarityClosed = table.Column<bool>(nullable: false, defaultValue: false),
                    CityId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attraction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attraction_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attraction_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_CategoryId",
                table: "Attraction",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_CityId",
                table: "Attraction",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketType_Attraction_AtrractionId",
                table: "TicketType",
                column: "AtrractionId",
                principalTable: "Attraction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingTime_Attraction_AtrractionId",
                table: "WorkingTime",
                column: "AtrractionId",
                principalTable: "Attraction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketType_Attraction_AtrractionId",
                table: "TicketType");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingTime_Attraction_AtrractionId",
                table: "WorkingTime");

            migrationBuilder.DropTable(
                name: "Attraction");

            migrationBuilder.CreateTable(
                name: "Atrraction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsTemporarityClosed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atrraction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atrraction_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Atrraction_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atrraction_CategoryId",
                table: "Atrraction",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Atrraction_CityId",
                table: "Atrraction",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketType_Atrraction_AtrractionId",
                table: "TicketType",
                column: "AtrractionId",
                principalTable: "Atrraction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingTime_Atrraction_AtrractionId",
                table: "WorkingTime",
                column: "AtrractionId",
                principalTable: "Atrraction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
