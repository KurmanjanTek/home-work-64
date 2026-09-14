using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace home_work_54_lab_work.Migrations
{
    /// <inheritdoc />
    public partial class AddFKExecutorIdAndCreatorIdToTaslItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "TaskItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExucutorId",
                table: "TaskItems",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "ExucutorId",
                table: "TaskItems");
        }
    }
}
