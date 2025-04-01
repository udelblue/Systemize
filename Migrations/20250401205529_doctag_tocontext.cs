using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Systemize.Migrations
{
    /// <inheritdoc />
    public partial class doctag_tocontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentTag_Document_DocumentID",
                table: "DocumentTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentTag",
                table: "DocumentTag");

            migrationBuilder.RenameTable(
                name: "DocumentTag",
                newName: "Document_Tags");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentTag_DocumentID",
                table: "Document_Tags",
                newName: "IX_Document_Tags_DocumentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Document_Tags",
                table: "Document_Tags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Tags_Document_DocumentID",
                table: "Document_Tags",
                column: "DocumentID",
                principalTable: "Document",
                principalColumn: "DocumentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Tags_Document_DocumentID",
                table: "Document_Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Document_Tags",
                table: "Document_Tags");

            migrationBuilder.RenameTable(
                name: "Document_Tags",
                newName: "DocumentTag");

            migrationBuilder.RenameIndex(
                name: "IX_Document_Tags_DocumentID",
                table: "DocumentTag",
                newName: "IX_DocumentTag_DocumentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentTag",
                table: "DocumentTag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentTag_Document_DocumentID",
                table: "DocumentTag",
                column: "DocumentID",
                principalTable: "Document",
                principalColumn: "DocumentID");
        }
    }
}
