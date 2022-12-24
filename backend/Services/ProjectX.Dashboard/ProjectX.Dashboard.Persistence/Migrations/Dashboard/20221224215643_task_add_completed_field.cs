using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectX.Dashboard.Persistence.Migrations.Dashboard
{
    /// <inheritdoc />
    public partial class taskaddcompletedfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                schema: "ProjectX.Dashboard",
                table: "Task",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                schema: "ProjectX.Dashboard",
                table: "Task");
        }
    }
}
