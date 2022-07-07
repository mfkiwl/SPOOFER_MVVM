using Microsoft.EntityFrameworkCore.Migrations;

namespace Spoofer.Migrations
{
    public partial class contextNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coordinates_User",
                table: "Coordinates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coordinates",
                table: "Coordinates");

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "UserId",
                keyValue: "abab69ea-3cd6-4d8f-973a-70c9c09bdce6");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CoorfianteId",
                table: "Coordinates");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "User",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Coordinates",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coordinates",
                table: "Coordinates",
                column: "Id");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "IsAuthenticated", "Password", "Permission", "UserName" },
                values: new object[] { "3730b8b1-6f31-4936-9abd-427c9e2960a4", null, "imsi400", null, "ori" });

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinates_User",
                table: "Coordinates",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coordinates_User",
                table: "Coordinates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coordinates",
                table: "Coordinates");

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "3730b8b1-6f31-4936-9abd-427c9e2960a4");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Coordinates");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CoorfianteId",
                table: "Coordinates",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coordinates",
                table: "Coordinates",
                column: "CoorfianteId");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "IsAuthenticated", "Password", "Permission", "UserName" },
                values: new object[] { "abab69ea-3cd6-4d8f-973a-70c9c09bdce6", null, "imsi400", null, "ori" });

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinates_User",
                table: "Coordinates",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
