using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyEvent.Migrations
{
    /// <inheritdoc />
    public partial class editToOneEventPerAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Events_AddressId",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_AddressId",
                table: "Events",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_AdminId",
                table: "Events",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_AdminId",
                table: "Events",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_AdminId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_AddressId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_AdminId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Events");

            migrationBuilder.CreateIndex(
                name: "IX_Events_AddressId",
                table: "Events",
                column: "AddressId");
        }
    }
}
