using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Systemize.Migrations
{
    /// <inheritdoc />
    public partial class updateCreatedByWF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workflow_User_WatchList_UserWatchListId",
                table: "Workflow");

            migrationBuilder.DropTable(
                name: "User_WatchList");

            migrationBuilder.DropIndex(
                name: "IX_Workflow_UserWatchListId",
                table: "Workflow");

            migrationBuilder.DropColumn(
                name: "UserWatchListId",
                table: "Workflow");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserWatchListId",
                table: "Workflow",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "User_WatchList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_WatchList", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workflow_UserWatchListId",
                table: "Workflow",
                column: "UserWatchListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workflow_User_WatchList_UserWatchListId",
                table: "Workflow",
                column: "UserWatchListId",
                principalTable: "User_WatchList",
                principalColumn: "Id");
        }
    }
}
