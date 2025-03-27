using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Systemize.Migrations
{
    /// <inheritdoc />
    public partial class extendedrevised6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "settingsId",
                table: "Workflow",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkflowSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowSetting", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workflow_settingsId",
                table: "Workflow",
                column: "settingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workflow_WorkflowSetting_settingsId",
                table: "Workflow",
                column: "settingsId",
                principalTable: "WorkflowSetting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workflow_WorkflowSetting_settingsId",
                table: "Workflow");

            migrationBuilder.DropTable(
                name: "WorkflowSetting");

            migrationBuilder.DropIndex(
                name: "IX_Workflow_settingsId",
                table: "Workflow");

            migrationBuilder.DropColumn(
                name: "settingsId",
                table: "Workflow");
        }
    }
}
