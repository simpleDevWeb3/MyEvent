using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyEvent.Migrations
{
    /// <inheritdoc />
    public partial class followedEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Details_DetailId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_DetailId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "DetailId",
                table: "Tickets");

            migrationBuilder.AddColumn<DateTime>(
                name: "FollowedDate",
                table: "FollowedEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FollowedEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowedDate",
                table: "FollowedEvents");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FollowedEvents");

            migrationBuilder.AddColumn<string>(
                name: "DetailId",
                table: "Tickets",
                type: "nvarchar(8)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_DetailId",
                table: "Tickets",
                column: "DetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Details_DetailId",
                table: "Tickets",
                column: "DetailId",
                principalTable: "Details",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
