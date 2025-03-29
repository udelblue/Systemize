using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Systemize.Migrations
{
    /// <inheritdoc />
    public partial class workflowtemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Workflow_WorkflowId",
                table: "Stage");

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowId",
                table: "Stage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "WorkflowTemplateId",
                table: "Stage",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Workflow_Template",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflow_Template", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stage_WorkflowTemplateId",
                table: "Stage",
                column: "WorkflowTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stage_Workflow_Template_WorkflowTemplateId",
                table: "Stage",
                column: "WorkflowTemplateId",
                principalTable: "Workflow_Template",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stage_Workflow_WorkflowId",
                table: "Stage",
                column: "WorkflowId",
                principalTable: "Workflow",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Workflow_Template_WorkflowTemplateId",
                table: "Stage");

            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Workflow_WorkflowId",
                table: "Stage");

            migrationBuilder.DropTable(
                name: "Workflow_Template");

            migrationBuilder.DropIndex(
                name: "IX_Stage_WorkflowTemplateId",
                table: "Stage");

            migrationBuilder.DropColumn(
                name: "WorkflowTemplateId",
                table: "Stage");

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowId",
                table: "Stage",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Stage_Workflow_WorkflowId",
                table: "Stage",
                column: "WorkflowId",
                principalTable: "Workflow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
