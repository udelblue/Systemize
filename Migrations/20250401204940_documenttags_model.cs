using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Systemize.Migrations
{
    /// <inheritdoc />
    public partial class documenttags_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentTag_Document_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "Document",
                        principalColumn: "DocumentID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTag_DocumentID",
                table: "DocumentTag",
                column: "DocumentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentTag");
        }
    }
}
