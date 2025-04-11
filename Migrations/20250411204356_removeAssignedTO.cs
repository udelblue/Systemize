using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Systemize.Migrations
{
    /// <inheritdoc />
    public partial class removeAssignedTO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "Stage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "Stage",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
