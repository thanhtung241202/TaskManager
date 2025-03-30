using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskPriorities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriorityId",
                table: "TaskItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskPriorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskPriorities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_PriorityId",
                table: "TaskItems",
                column: "PriorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_TaskPriorities_PriorityId",
                table: "TaskItems",
                column: "PriorityId",
                principalTable: "TaskPriorities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_TaskPriorities_PriorityId",
                table: "TaskItems");

            migrationBuilder.DropTable(
                name: "TaskPriorities");

            migrationBuilder.DropIndex(
                name: "IX_TaskItems_PriorityId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "PriorityId",
                table: "TaskItems");
        }
    }
}
