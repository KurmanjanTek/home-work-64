using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace home_work_54_lab_work.Migrations
{
    /// <inheritdoc />
    public partial class RemoveExecutorName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecuterName",
                table: "TaskItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExecuterName",
                table: "TaskItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
