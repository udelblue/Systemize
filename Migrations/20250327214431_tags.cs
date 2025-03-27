using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Systemize.Migrations
{
    /// <inheritdoc />
    public partial class tags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Workflow_WorkflowId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_History_Workflow_WorkflowId",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_Link_Workflow_WorkflowId",
                table: "Link");

            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Workflow_WorkflowId",
                table: "Stage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workflow",
                table: "Workflow");

            migrationBuilder.RenameTable(
                name: "Workflow",
                newName: "Workflow_Tags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workflow_Tags",
                table: "Workflow_Tags",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "WorkflowTag",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkflowId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowTag_Workflow_Tags_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflow_Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTag_WorkflowId",
                table: "WorkflowTag",
                column: "WorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Workflow_Tags_WorkflowId",
                table: "Document",
                column: "WorkflowId",
                principalTable: "Workflow_Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_History_Workflow_Tags_WorkflowId",
                table: "History",
                column: "WorkflowId",
                principalTable: "Workflow_Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Link_Workflow_Tags_WorkflowId",
                table: "Link",
                column: "WorkflowId",
                principalTable: "Workflow_Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stage_Workflow_Tags_WorkflowId",
                table: "Stage",
                column: "WorkflowId",
                principalTable: "Workflow_Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Workflow_Tags_WorkflowId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_History_Workflow_Tags_WorkflowId",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_Link_Workflow_Tags_WorkflowId",
                table: "Link");

            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Workflow_Tags_WorkflowId",
                table: "Stage");

            migrationBuilder.DropTable(
                name: "WorkflowTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workflow_Tags",
                table: "Workflow_Tags");

            migrationBuilder.RenameTable(
                name: "Workflow_Tags",
                newName: "Workflow");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workflow",
                table: "Workflow",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Workflow_WorkflowId",
                table: "Document",
                column: "WorkflowId",
                principalTable: "Workflow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_History_Workflow_WorkflowId",
                table: "History",
                column: "WorkflowId",
                principalTable: "Workflow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Link_Workflow_WorkflowId",
                table: "Link",
                column: "WorkflowId",
                principalTable: "Workflow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
