using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PupUp.Migrations
{
    public partial class profil_update_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilImageUrl",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilImageUrl",
                table: "AspNetUsers");
        }
    }
}
