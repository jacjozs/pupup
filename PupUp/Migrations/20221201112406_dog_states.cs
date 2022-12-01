using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PupUp.Migrations
{
    public partial class dog_states : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserQuest",
                table: "Quests",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UserBadge",
                table: "Badges",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DogBadges",
                columns: table => new
                {
                    BadgeId = table.Column<int>(type: "INTEGER", nullable: false),
                    DogId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DogBadges", x => new { x.BadgeId, x.DogId });
                    table.ForeignKey(
                        name: "FK_DogBadges_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DogBadges_Dogs_DogId",
                        column: x => x.DogId,
                        principalTable: "Dogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DogQuests",
                columns: table => new
                {
                    QuestId = table.Column<int>(type: "INTEGER", nullable: false),
                    DogId = table.Column<int>(type: "INTEGER", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DogQuests", x => new { x.QuestId, x.DogId });
                    table.ForeignKey(
                        name: "FK_DogQuests_Dogs_DogId",
                        column: x => x.DogId,
                        principalTable: "Dogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DogQuests_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DogBadges_DogId",
                table: "DogBadges",
                column: "DogId");

            migrationBuilder.CreateIndex(
                name: "IX_DogQuests_DogId",
                table: "DogQuests",
                column: "DogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DogBadges");

            migrationBuilder.DropTable(
                name: "DogQuests");

            migrationBuilder.DropColumn(
                name: "UserQuest",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "UserBadge",
                table: "Badges");
        }
    }
}
