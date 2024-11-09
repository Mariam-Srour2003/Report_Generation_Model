using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSomeDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolesPermissions",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Name",
                value: "Signup");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Name",
                value: "Login");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 8L,
                column: "Name",
                value: "UploadImage");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 9L,
                column: "Name",
                value: "ManageReports");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10L,
                column: "Name",
                value: "ManageXRayImages");

            migrationBuilder.UpdateData(
                table: "RolesPermissions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "PermissionId",
                value: 7L);

            migrationBuilder.UpdateData(
                table: "RolesPermissions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "PermissionId",
                value: 7L);

            migrationBuilder.UpdateData(
                table: "RolesPermissions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "RoleId",
                value: 2L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Name",
                value: "DownloadReports");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Name",
                value: "Signup");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 8L,
                column: "Name",
                value: "Login");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 9L,
                column: "Name",
                value: "UploadImage");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10L,
                column: "Name",
                value: "ManageReports");

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[] { 11L, "ManageXRayImages" });

            migrationBuilder.UpdateData(
                table: "RolesPermissions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "PermissionId",
                value: 8L);

            migrationBuilder.UpdateData(
                table: "RolesPermissions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "PermissionId",
                value: 8L);

            migrationBuilder.UpdateData(
                table: "RolesPermissions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "RoleId",
                value: 3L);

            migrationBuilder.InsertData(
                table: "RolesPermissions",
                columns: new[] { "Id", "PermissionId", "RoleId" },
                values: new object[] { 13L, 11L, 2L });
        }
    }
}
