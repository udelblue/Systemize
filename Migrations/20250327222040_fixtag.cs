using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Systemize.Migrations
{
    /// <inheritdoc />
    public partial class fixtag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Workflow_Tags");

            migrationBuilder.DropColumn(
                name: "CurrentlyAssigned",
                table: "Workflow_Tags");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Workflow_Tags");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Workflow_Tags");

            migrationBuilder.RenameColumn(
                name: "CurrentStageId",
                table: "Workflow_Tags",
                newName: "WorkflowId");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Workflow_Tags",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "Workflow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentlyAssigned = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentStageId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflow", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workflow_Tags_WorkflowId",
                table: "Workflow_Tags",
                column: "WorkflowId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Workflow_Tags_Workflow_WorkflowId",
                table: "Workflow_Tags",
                column: "WorkflowId",
                principalTable: "Workflow",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_Workflow_Tags_Workflow_WorkflowId",
                table: "Workflow_Tags");

            migrationBuilder.DropTable(
                name: "Workflow");

            migrationBuilder.DropIndex(
                name: "IX_Workflow_Tags_WorkflowId",
                table: "Workflow_Tags");

            migrationBuilder.RenameColumn(
                name: "WorkflowId",
                table: "Workflow_Tags",
                newName: "CurrentStageId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Workflow_Tags",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Workflow_Tags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentlyAssigned",
                table: "Workflow_Tags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Workflow_Tags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Workflow_Tags",
                type: "nvarchar(max)",
                nullable: true);

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
    }
}
