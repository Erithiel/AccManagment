using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccManagment.API.Migrations
{
    public partial class addingSecretCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "secretCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "secretCode",
                table: "Users");
        }
    }
}
