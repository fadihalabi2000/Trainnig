﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewsApiData;

#nullable disable

namespace NewsApiData.Migrations
{
    [DbContext(typeof(NewsApiDbContext))]
    partial class NewsApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("NewsApiDomin.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ViewCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Articles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = 1,
                            CategoryId = 1,
                            Content = "أصيب أكثر من 200 فلسطيني في مواجهات بنابلس مع قوات الاحتلال الإسرائيلي، بالتزامن مع تشييع شهيد بأريحا، وبعد ساعات من اقتحام مئات المستوطنين للمسجد الأقصى المبارك في القدس.",
                            IsDeleted = false,
                            PublishDate = new DateTime(2023, 12, 4, 17, 54, 44, 565, DateTimeKind.Utc).AddTicks(6125),
                            Title = "فلسطين.. عشرات الإصابات بنابلس وتشييع شهيد بأريحا واقتحام يهودي للأقصى",
                            UpdateDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ViewCount = 0
                        },
                        new
                        {
                            Id = 2,
                            AuthorId = 2,
                            CategoryId = 2,
                            Content = "اقتحم مستوطنون باحات المسجد الأقصى صباح اليوم الأحد بحماية قوات الاحتلال الإسرائيلي التي اعتدت على شبان فلسطينيين عند باب الأسباط ومنعتهم بالقوة من دخول المسجد الأقصى لأداء صلاة فجر اليوم، كما منعت عشرات النساء من الدخول.",
                            IsDeleted = false,
                            PublishDate = new DateTime(2023, 12, 4, 17, 54, 44, 565, DateTimeKind.Utc).AddTicks(6133),
                            Title = "الاحتلال يعتدي على فلسطينيين ويمنعهم من دخول الأقصى",
                            UpdateDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ViewCount = 0
                        },
                        new
                        {
                            Id = 3,
                            AuthorId = 3,
                            CategoryId = 3,
                            Content = "منذ بداية الشهر الجاري هجمات على ضباط في الجيش والقوات الأمنية أدت إلى مقتل ضابط في الجيش وآخر في الشرطة وثالث في الدعم السريع ونهب مركبات عسكرية،",
                            IsDeleted = false,
                            PublishDate = new DateTime(2023, 12, 4, 17, 54, 44, 565, DateTimeKind.Utc).AddTicks(6136),
                            Title = "حوادث اغتيال العسكريين السودانيين",
                            UpdateDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ViewCount = 0
                        });
                });

            modelBuilder.Entity("NewsApiDomin.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Bio = "aaaaa",
                            DisplayName = "ali",
                            Email = "ali@gmail.com",
                            IsDeleted = false,
                            Password = "123",
                            ProfilePicture = "https://tse4.mm.bing.net/th/id/OIP.y2TOfKrvLx09_tbuortEygHaEG?w=331&h=183&c=7&r=0&o=5&pid=1.7"
                        },
                        new
                        {
                            Id = 2,
                            Bio = "ooooo",
                            DisplayName = "omar",
                            Email = "omar@gmail.com",
                            IsDeleted = false,
                            Password = "145",
                            ProfilePicture = "https://tse4.mm.bing.net/th/id/OIP.1FMDAFhu9UEmpewQZBWfqgHaEK?w=326&h=183&c=7&r=0&o=5&pid=1.7"
                        },
                        new
                        {
                            Id = 3,
                            Bio = "hhhhh",
                            DisplayName = "ahmad",
                            Email = "ahmad@gmail.com",
                            IsDeleted = false,
                            Password = "165",
                            ProfilePicture = "https://tse1.mm.bing.net/th/id/OIP.U8tBnyvXfaWfsx3Q-cIXUAHaHa?w=180&h=180&c=7&r=0&o=5&pid=1.7"
                        });
                });

            modelBuilder.Entity("NewsApiDomin.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryName = "sport",
                            IsDeleted = false
                        },
                        new
                        {
                            Id = 2,
                            CategoryName = "Political",
                            IsDeleted = false
                        },
                        new
                        {
                            Id = 3,
                            CategoryName = "Military",
                            IsDeleted = false
                        });
                });

            modelBuilder.Entity("NewsApiDomin.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CommentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CommentText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ArticleId = 1,
                            CommentDate = new DateTime(2023, 12, 4, 17, 54, 44, 565, DateTimeKind.Utc).AddTicks(6181),
                            CommentText = "wwwww",
                            IsDeleted = false,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            ArticleId = 2,
                            CommentDate = new DateTime(2023, 12, 4, 17, 54, 44, 565, DateTimeKind.Utc).AddTicks(6184),
                            CommentText = "sssss",
                            IsDeleted = false,
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            ArticleId = 3,
                            CommentDate = new DateTime(2023, 12, 4, 17, 54, 44, 565, DateTimeKind.Utc).AddTicks(6186),
                            CommentText = "xxxxxxx",
                            IsDeleted = false,
                            UserId = 3
                        });
                });

            modelBuilder.Entity("NewsApiDomin.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<string>("ImageDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Images");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ArticleId = 1,
                            ImageDescription = "no",
                            ImageUrl = "https://tse4.mm.bing.net/th/id/OIP.P-lDxR5o6Hatd2C5RBKukAHaEO?w=263&h=180&c=7&r=0&o=5&pid=1.7",
                            IsDeleted = false
                        },
                        new
                        {
                            Id = 2,
                            ArticleId = 2,
                            ImageDescription = "no",
                            ImageUrl = "https://tse2.mm.bing.net/th/id/OIP.W2fvNzcjgTB7zbO9NDRXSwHaFL?w=212&h=180&c=7&r=0&o=5&pid=1.7",
                            IsDeleted = false
                        },
                        new
                        {
                            Id = 3,
                            ArticleId = 3,
                            ImageDescription = "no",
                            ImageUrl = "https://tse4.mm.bing.net/th/id/OIP.5zlHy1zk4adkwBWLRxVUqgHaFE?w=233&h=180&c=7&r=0&o=5&pid=1.7",
                            IsDeleted = false
                        });
                });

            modelBuilder.Entity("NewsApiDomin.Models.Like", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LikeDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ArticleId = 1,
                            IsDeleted = false,
                            LikeDate = new DateTime(2023, 12, 4, 17, 54, 44, 565, DateTimeKind.Utc).AddTicks(6159),
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            ArticleId = 2,
                            IsDeleted = false,
                            LikeDate = new DateTime(2023, 12, 4, 17, 54, 44, 565, DateTimeKind.Utc).AddTicks(6161),
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            ArticleId = 3,
                            IsDeleted = false,
                            LikeDate = new DateTime(2023, 12, 4, 17, 54, 44, 565, DateTimeKind.Utc).AddTicks(6162),
                            UserId = 3
                        });
                });

            modelBuilder.Entity("NewsApiDomin.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("logLevel")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("UserId");

                    b.ToTable("Logs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = 1,
                            Content = "add",
                            DateCreated = new DateTime(2023, 12, 4, 17, 54, 44, 565, DateTimeKind.Utc).AddTicks(6226),
                            IsDeleted = false,
                            logLevel = 2
                        },
                        new
                        {
                            Id = 2,
                            AuthorId = 1,
                            Content = "delete",
                            DateCreated = new DateTime(2023, 12, 4, 17, 54, 44, 565, DateTimeKind.Utc).AddTicks(6230),
                            IsDeleted = false,
                            logLevel = 2
                        },
                        new
                        {
                            Id = 3,
                            Content = "update",
                            DateCreated = new DateTime(2023, 12, 4, 17, 54, 44, 565, DateTimeKind.Utc).AddTicks(6232),
                            IsDeleted = false,
                            UserId = 1,
                            logLevel = 2
                        });
                });

            modelBuilder.Entity("NewsApiDomin.Models.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("RevokedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("NewsApiDomin.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayName = "obada",
                            Email = "obada@gmail.com",
                            FirstName = "obada",
                            IsDeleted = false,
                            LastName = "halabi",
                            Password = "12345",
                            ProfilePicture = "https://www.bing.com/th?id=OIP.RYDAyx95XZfKlV4Utf8Z7QHaEK&w=333&h=187&c=8&rs=1&qlt=90&o=6&pid=3.1&rm=2"
                        },
                        new
                        {
                            Id = 2,
                            DisplayName = "fadi",
                            Email = "fadi@gmail.com",
                            FirstName = "fadi",
                            IsDeleted = false,
                            LastName = "halabi",
                            Password = "123456",
                            ProfilePicture = "https://www.bing.com/th?id=OIP.RYDAyx95XZfKlV4Utf8Z7QHaEK&w=333&h=187&c=8&rs=1&qlt=90&o=6&pid=3.1&rm=2"
                        },
                        new
                        {
                            Id = 3,
                            DisplayName = "taher",
                            Email = "taher@gmail.com",
                            FirstName = "taher",
                            IsDeleted = false,
                            LastName = "halabi",
                            Password = "12345",
                            ProfilePicture = "https://www.bing.com/th?id=OIP.frAlEuXSfGFRLcBxzVRY1AHaER&w=329&h=189&c=8&rs=1&qlt=90&o=6&pid=3.1&rm=2"
                        });
                });

            modelBuilder.Entity("NewsApiDomin.Models.Article", b =>
                {
                    b.HasOne("NewsApiDomin.Models.Author", "Author")
                        .WithMany("Article")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewsApiDomin.Models.Category", "Category")
                        .WithMany("Articles")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("NewsApiDomin.Models.Author", b =>
                {
                    b.HasOne("NewsApiDomin.Models.User", null)
                        .WithMany("Authors")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("NewsApiDomin.Models.Comment", b =>
                {
                    b.HasOne("NewsApiDomin.Models.Article", null)
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewsApiDomin.Models.User", null)
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NewsApiDomin.Models.Image", b =>
                {
                    b.HasOne("NewsApiDomin.Models.Article", null)
                        .WithMany("Images")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NewsApiDomin.Models.Like", b =>
                {
                    b.HasOne("NewsApiDomin.Models.Article", null)
                        .WithMany("Likes")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NewsApiDomin.Models.User", null)
                        .WithMany("likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NewsApiDomin.Models.Log", b =>
                {
                    b.HasOne("NewsApiDomin.Models.Author", "Author")
                        .WithMany("Log")
                        .HasForeignKey("AuthorId");

                    b.HasOne("NewsApiDomin.Models.User", "User")
                        .WithMany("Logs")
                        .HasForeignKey("UserId");

                    b.Navigation("Author");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NewsApiDomin.Models.RefreshToken", b =>
                {
                    b.HasOne("NewsApiDomin.Models.Author", "Author")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("AuthorId");

                    b.HasOne("NewsApiDomin.Models.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId");

                    b.Navigation("Author");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NewsApiDomin.Models.Article", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Images");

                    b.Navigation("Likes");
                });

            modelBuilder.Entity("NewsApiDomin.Models.Author", b =>
                {
                    b.Navigation("Article");

                    b.Navigation("Log");

                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("NewsApiDomin.Models.Category", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("NewsApiDomin.Models.User", b =>
                {
                    b.Navigation("Authors");

                    b.Navigation("Comments");

                    b.Navigation("Logs");

                    b.Navigation("RefreshTokens");

                    b.Navigation("likes");
                });
#pragma warning restore 612, 618
        }
    }
}
