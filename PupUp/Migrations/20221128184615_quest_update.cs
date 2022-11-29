using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PupUp.Migrations
{
    public partial class quest_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserQuests_QuestId",
                table: "UserQuests");

            migrationBuilder.DropIndex(
                name: "IX_UserQuests_UserId",
                table: "UserQuests");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuests_UserId",
                table: "UserQuests",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserQuests_UserId",
                table: "UserQuests");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuests_QuestId",
                table: "UserQuests",
                column: "QuestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserQuests_UserId",
                table: "UserQuests",
                column: "UserId",
                unique: true);
        }
    }
}
