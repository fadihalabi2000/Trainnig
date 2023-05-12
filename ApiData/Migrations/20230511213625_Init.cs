using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsApiData.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    logLevel = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Logs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LikeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Bio", "DisplayName", "Email", "IsDeleted", "Password", "ProfilePicture", "UserId" },
                values: new object[,]
                {
                    { 1, "aaaaa", "ali", "ali@gmail.com", false, "123", "https://tse4.mm.bing.net/th/id/OIP.y2TOfKrvLx09_tbuortEygHaEG?w=331&h=183&c=7&r=0&o=5&pid=1.7", null },
                    { 2, "ooooo", "omar", "omar@gmail.com", false, "145", "https://tse4.mm.bing.net/th/id/OIP.1FMDAFhu9UEmpewQZBWfqgHaEK?w=326&h=183&c=7&r=0&o=5&pid=1.7", null },
                    { 3, "hhhhh", "ahmad", "ahmad@gmail.com", false, "165", "https://tse1.mm.bing.net/th/id/OIP.U8tBnyvXfaWfsx3Q-cIXUAHaHa?w=180&h=180&c=7&r=0&o=5&pid=1.7", null }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "IsDeleted" },
                values: new object[,]
                {
                    { 1, "sport", false },
                    { 2, "Political", false },
                    { 3, "Military", false }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DisplayName", "Email", "FirstName", "IsDeleted", "LastName", "Password", "ProfilePicture" },
                values: new object[,]
                {
                    { 1, "obada", "obada@gmail.com", "obada", false, "halabi", "12345", "https://www.bing.com/th?id=OIP.RYDAyx95XZfKlV4Utf8Z7QHaEK&w=333&h=187&c=8&rs=1&qlt=90&o=6&pid=3.1&rm=2" },
                    { 2, "fadi", "fadi@gmail.com", "fadi", false, "halabi", "123456", "https://www.bing.com/th?id=OIP.RYDAyx95XZfKlV4Utf8Z7QHaEK&w=333&h=187&c=8&rs=1&qlt=90&o=6&pid=3.1&rm=2" },
                    { 3, "taher", "taher@gmail.com", "taher", false, "halabi", "12345", "https://www.bing.com/th?id=OIP.frAlEuXSfGFRLcBxzVRY1AHaER&w=329&h=189&c=8&rs=1&qlt=90&o=6&pid=3.1&rm=2" }
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "AuthorId", "CategoryId", "Content", "IsDeleted", "PublishDate", "Title", "UpdateDate", "ViewCount" },
                values: new object[,]
                {
                    { 1, 1, 1, "أصيب أكثر من 200 فلسطيني في مواجهات بنابلس مع قوات الاحتلال الإسرائيلي، بالتزامن مع تشييع شهيد بأريحا، وبعد ساعات من اقتحام مئات المستوطنين للمسجد الأقصى المبارك في القدس.", false, new DateTime(2023, 5, 11, 21, 36, 25, 600, DateTimeKind.Utc).AddTicks(2495), "فلسطين.. عشرات الإصابات بنابلس وتشييع شهيد بأريحا واقتحام يهودي للأقصى", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 2, 2, 2, "اقتحم مستوطنون باحات المسجد الأقصى صباح اليوم الأحد بحماية قوات الاحتلال الإسرائيلي التي اعتدت على شبان فلسطينيين عند باب الأسباط ومنعتهم بالقوة من دخول المسجد الأقصى لأداء صلاة فجر اليوم، كما منعت عشرات النساء من الدخول.", false, new DateTime(2023, 5, 11, 21, 36, 25, 600, DateTimeKind.Utc).AddTicks(2501), "الاحتلال يعتدي على فلسطينيين ويمنعهم من دخول الأقصى", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 3, 3, 3, "منذ بداية الشهر الجاري هجمات على ضباط في الجيش والقوات الأمنية أدت إلى مقتل ضابط في الجيش وآخر في الشرطة وثالث في الدعم السريع ونهب مركبات عسكرية،", false, new DateTime(2023, 5, 11, 21, 36, 25, 600, DateTimeKind.Utc).AddTicks(2502), "حوادث اغتيال العسكريين السودانيين", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 }
                });

            migrationBuilder.InsertData(
                table: "Logs",
                columns: new[] { "Id", "AuthorId", "Content", "DateCreated", "IsDeleted", "UserId", "logLevel" },
                values: new object[,]
                {
                    { 1, 1, "add", new DateTime(2023, 5, 11, 21, 36, 25, 600, DateTimeKind.Utc).AddTicks(2559), false, null, 2 },
                    { 2, 1, "delete", new DateTime(2023, 5, 11, 21, 36, 25, 600, DateTimeKind.Utc).AddTicks(2561), false, null, 2 },
                    { 3, null, "update", new DateTime(2023, 5, 11, 21, 36, 25, 600, DateTimeKind.Utc).AddTicks(2562), false, 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "ArticleId", "CommentDate", "CommentText", "IsDeleted", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 5, 11, 21, 36, 25, 600, DateTimeKind.Utc).AddTicks(2529), "wwwww", false, 1 },
                    { 2, 2, new DateTime(2023, 5, 11, 21, 36, 25, 600, DateTimeKind.Utc).AddTicks(2531), "sssss", false, 2 },
                    { 3, 3, new DateTime(2023, 5, 11, 21, 36, 25, 600, DateTimeKind.Utc).AddTicks(2532), "xxxxxxx", false, 3 }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "ArticleId", "ImageDescription", "ImageUrl", "IsDeleted" },
                values: new object[,]
                {
                    { 1, 1, "no", "https://tse4.mm.bing.net/th/id/OIP.P-lDxR5o6Hatd2C5RBKukAHaEO?w=263&h=180&c=7&r=0&o=5&pid=1.7", false },
                    { 2, 2, "no", "https://tse2.mm.bing.net/th/id/OIP.W2fvNzcjgTB7zbO9NDRXSwHaFL?w=212&h=180&c=7&r=0&o=5&pid=1.7", false },
                    { 3, 3, "no", "https://tse4.mm.bing.net/th/id/OIP.5zlHy1zk4adkwBWLRxVUqgHaFE?w=233&h=180&c=7&r=0&o=5&pid=1.7", false }
                });

            migrationBuilder.InsertData(
                table: "Likes",
                columns: new[] { "Id", "ArticleId", "IsDeleted", "LikeDate", "UserId" },
                values: new object[,]
                {
                    { 1, 1, false, new DateTime(2023, 5, 11, 21, 36, 25, 600, DateTimeKind.Utc).AddTicks(2516), 1 },
                    { 2, 2, false, new DateTime(2023, 5, 11, 21, 36, 25, 600, DateTimeKind.Utc).AddTicks(2517), 2 },
                    { 3, 3, false, new DateTime(2023, 5, 11, 21, 36, 25, 600, DateTimeKind.Utc).AddTicks(2518), 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorId",
                table: "Articles",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_UserId",
                table: "Authors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ArticleId",
                table: "Images",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ArticleId",
                table: "Likes",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_AuthorId",
                table: "Logs",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UserId",
                table: "Logs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_AuthorId",
                table: "RefreshTokens",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
