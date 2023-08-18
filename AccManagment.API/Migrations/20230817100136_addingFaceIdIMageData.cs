using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccManagment.API.Migrations
{
    public partial class addingFaceIdIMageData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FaceIdImageData",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceIdImageData",
                table: "Users");
        }
    }
}
