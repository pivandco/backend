using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAutomation.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f934a554-cc74-47d3-b872-45e9f138f7fe", 0, "52611899-3c2f-42e5-9888-54bd62de3944", null, false, false, null, null, null, "AQAAAAIAAYagAAAAEJcFpRklDx+/0pNDaAEpWLjbdovDgqSy/X+9LY8+U5VGzapnfTr3g0jZE6JM3mM/9Q==", null, false, "7d60f339-55eb-4140-b9be-2ffa8a96f072", false, "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f934a554-cc74-47d3-b872-45e9f138f7fe");
        }
    }
}
