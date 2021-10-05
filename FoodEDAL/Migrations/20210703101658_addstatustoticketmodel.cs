using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodEDAL.Migrations
{
    public partial class addstatustoticketmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SupportTickets",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "SupportTickets");
        }
    }
}
