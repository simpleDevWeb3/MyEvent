using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyEvent.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowedEventTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FollowedEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<string>(type: "nvarchar(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowedEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowedEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowedEvents_EventId",
                table: "FollowedEvents",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowedEvents");
        }
    }
}
