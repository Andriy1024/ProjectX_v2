using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectX.Identity.Persistence.Migrations.Identity
{
    /// <inheritdoc />
    public partial class RenameIsRevoked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId1",
                schema: "ProjectX.Identity",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId1",
                schema: "ProjectX.Identity",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_RoleId1",
                schema: "ProjectX.Identity",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId1",
                schema: "ProjectX.Identity",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                schema: "ProjectX.Identity",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                schema: "ProjectX.Identity",
                table: "AspNetUserRoles");

            migrationBuilder.RenameColumn(
                name: "IsRevorked",
                schema: "ProjectX.Identity",
                table: "RefreshToken",
                newName: "IsRevoked");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRevoked",
                schema: "ProjectX.Identity",
                table: "RefreshToken",
                newName: "IsRevorked");

            migrationBuilder.AddColumn<int>(
                name: "RoleId1",
                schema: "ProjectX.Identity",
                table: "AspNetUserRoles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                schema: "ProjectX.Identity",
                table: "AspNetUserRoles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId1",
                schema: "ProjectX.Identity",
                table: "AspNetUserRoles",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId1",
                schema: "ProjectX.Identity",
                table: "AspNetUserRoles",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId1",
                schema: "ProjectX.Identity",
                table: "AspNetUserRoles",
                column: "RoleId1",
                principalSchema: "ProjectX.Identity",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId1",
                schema: "ProjectX.Identity",
                table: "AspNetUserRoles",
                column: "UserId1",
                principalSchema: "ProjectX.Identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
