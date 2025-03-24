using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Systemize.Migrations
{
    /// <inheritdoc />
    public partial class document_types : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Workflow_WorkflowId",
                table: "Document");

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowId",
                table: "Document",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentType",
                table: "Document",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Workflow_WorkflowId",
                table: "Document",
                column: "WorkflowId",
                principalTable: "Workflow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Workflow_WorkflowId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "Document");

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowId",
                table: "Document",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Workflow_WorkflowId",
                table: "Document",
                column: "WorkflowId",
                principalTable: "Workflow",
                principalColumn: "Id");
        }
    }
}
