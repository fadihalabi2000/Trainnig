using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsApiData.Migrations
{
    public partial class Addseed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "PublishDate",
                value: new DateTime(2023, 4, 11, 12, 24, 37, 522, DateTimeKind.Utc).AddTicks(8694));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "PublishDate",
                value: new DateTime(2023, 4, 11, 12, 24, 37, 522, DateTimeKind.Utc).AddTicks(8700));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "PublishDate",
                value: new DateTime(2023, 4, 11, 12, 24, 37, 522, DateTimeKind.Utc).AddTicks(8702));

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Bio", "DisplayName", "Email", "IsDeleted", "Password", "ProfilePicture" },
                values: new object[] { 2147483647, "test", "test", "testA@gmail.com", false, "165", "https://tse1.mm.bing.net/th/id/OIP.U8tBnyvXfaWfsx3Q-cIXUAHaHa?w=180&h=180&c=7&r=0&o=5&pid=1.7" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CommentDate",
                value: new DateTime(2023, 4, 11, 12, 24, 37, 522, DateTimeKind.Utc).AddTicks(8735));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CommentDate",
                value: new DateTime(2023, 4, 11, 12, 24, 37, 522, DateTimeKind.Utc).AddTicks(8737));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CommentDate",
                value: new DateTime(2023, 4, 11, 12, 24, 37, 522, DateTimeKind.Utc).AddTicks(8738));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 1,
                column: "LikeDate",
                value: new DateTime(2023, 4, 11, 12, 24, 37, 522, DateTimeKind.Utc).AddTicks(8717));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 2,
                column: "LikeDate",
                value: new DateTime(2023, 4, 11, 12, 24, 37, 522, DateTimeKind.Utc).AddTicks(8718));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 3,
                column: "LikeDate",
                value: new DateTime(2023, 4, 11, 12, 24, 37, 522, DateTimeKind.Utc).AddTicks(8719));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Email",
                value: "taher@gmail.com");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DisplayName", "Email", "FirstName", "IsDeleted", "LastName", "Password", "ProfilePicture" },
                values: new object[] { 2147483647, "test", "test@gmail.com", "test", true, "halabi", "12345", "https://www.bing.com/th?id=OIP.frAlEuXSfGFRLcBxzVRY1AHaER&w=329&h=189&c=8&rs=1&qlt=90&o=6&pid=3.1&rm=2" });

            migrationBuilder.InsertData(
                table: "Logs",
                columns: new[] { "Id", "AuthorId", "Content", "DateCreated", "IsDeleted", "UserId", "logLevel" },
                values: new object[] { 1, 1, "add", new DateTime(2023, 4, 11, 12, 24, 37, 522, DateTimeKind.Utc).AddTicks(8766), false, 2147483647, 2 });

            migrationBuilder.InsertData(
                table: "Logs",
                columns: new[] { "Id", "AuthorId", "Content", "DateCreated", "IsDeleted", "UserId", "logLevel" },
                values: new object[] { 2, 1, "delete", new DateTime(2023, 4, 11, 12, 24, 37, 522, DateTimeKind.Utc).AddTicks(8769), false, 2147483647, 2 });

            migrationBuilder.InsertData(
                table: "Logs",
                columns: new[] { "Id", "AuthorId", "Content", "DateCreated", "IsDeleted", "UserId", "logLevel" },
                values: new object[] { 3, 2147483647, "update", new DateTime(2023, 4, 11, 12, 24, 37, 522, DateTimeKind.Utc).AddTicks(8770), false, 1, 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Logs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Logs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Logs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2147483647);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2147483647);

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "PublishDate",
                value: new DateTime(2023, 4, 10, 21, 24, 51, 963, DateTimeKind.Utc).AddTicks(1855));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "PublishDate",
                value: new DateTime(2023, 4, 10, 21, 24, 51, 963, DateTimeKind.Utc).AddTicks(1861));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "PublishDate",
                value: new DateTime(2023, 4, 10, 21, 24, 51, 963, DateTimeKind.Utc).AddTicks(1863));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CommentDate",
                value: new DateTime(2023, 4, 10, 21, 24, 51, 963, DateTimeKind.Utc).AddTicks(1896));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CommentDate",
                value: new DateTime(2023, 4, 10, 21, 24, 51, 963, DateTimeKind.Utc).AddTicks(1897));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CommentDate",
                value: new DateTime(2023, 4, 10, 21, 24, 51, 963, DateTimeKind.Utc).AddTicks(1898));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 1,
                column: "LikeDate",
                value: new DateTime(2023, 4, 10, 21, 24, 51, 963, DateTimeKind.Utc).AddTicks(1879));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 2,
                column: "LikeDate",
                value: new DateTime(2023, 4, 10, 21, 24, 51, 963, DateTimeKind.Utc).AddTicks(1881));

            migrationBuilder.UpdateData(
                table: "Likes",
                keyColumn: "Id",
                keyValue: 3,
                column: "LikeDate",
                value: new DateTime(2023, 4, 10, 21, 24, 51, 963, DateTimeKind.Utc).AddTicks(1882));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Email",
                value: "taher");
        }
    }
}
