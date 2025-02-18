using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoginRegister.Migrations
{
    /// <inheritdoc />
    public partial class three : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c85dc5ec-92b8-4af6-a3cb-edfdf9b42e22");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cad72e8b-facc-4440-a680-5d57da24bf6b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "695b396e-91ea-40ec-866a-c0c25d60e34f", null, "Администратор", "Администратор" },
                    { "da64fc9c-2cdd-44b0-94d1-2ea3227ad9ad", null, "Клиент", "Клиент" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "695b396e-91ea-40ec-866a-c0c25d60e34f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "da64fc9c-2cdd-44b0-94d1-2ea3227ad9ad");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c85dc5ec-92b8-4af6-a3cb-edfdf9b42e22", null, "Администратор", "Администратор" },
                    { "cad72e8b-facc-4440-a680-5d57da24bf6b", null, "Клиент", "Клиент" }
                });
        }
    }
}
