using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Systemize.Migrations
{
    /// <inheritdoc />
    public partial class wf_created : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_History_Workflow_WorkflowId",
                table: "History");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Workflow",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowId",
                table: "History",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_History_Workflow_WorkflowId",
                table: "History",
                column: "WorkflowId",
                principalTable: "Workflow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_History_Workflow_WorkflowId",
                table: "History");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Workflow");

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowId",
                table: "History",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_History_Workflow_WorkflowId",
                table: "History",
                column: "WorkflowId",
                principalTable: "Workflow",
                principalColumn: "Id");
        }
    }
}
