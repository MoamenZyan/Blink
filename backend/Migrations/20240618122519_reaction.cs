using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class reaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ReactionReplies");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ReactionPosts");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ReactionComments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Type",
                table: "ReactionReplies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Type",
                table: "ReactionPosts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Type",
                table: "ReactionComments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
