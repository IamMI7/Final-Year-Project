using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodEDAL.Migrations
{
    public partial class updatetipsmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipTitle",
                table: "Tips",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipTitle",
                table: "Tips");
        }
    }
}
