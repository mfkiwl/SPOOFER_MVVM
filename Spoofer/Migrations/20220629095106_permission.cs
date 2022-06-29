using Microsoft.EntityFrameworkCore.Migrations;

namespace Spoofer.Migrations
{
    public partial class permission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Permission",
                table: "User",
                nullable: true);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "IsAuthenticated", "Password", "Permission", "UserName" },
                values: new object[] { "abab69ea-3cd6-4d8f-973a-70c9c09bdce6", null, "imsi400", null, "ori" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "UserId",
                keyValue: "abab69ea-3cd6-4d8f-973a-70c9c09bdce6");

            migrationBuilder.DropColumn(
                name: "Permission",
                table: "User");
        }
    }
}
