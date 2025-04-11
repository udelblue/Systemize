using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Systemize.Migrations
{
    /// <inheritdoc />
    public partial class addAssignedtoList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "Stage",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "Stage");
        }
    }
}
