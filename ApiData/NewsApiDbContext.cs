using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsApiDomin.Models;
using System.Xml;

namespace NewsApiData
{
    public class NewsApiDbContext : DbContext
    {
        public  NewsApiDbContext(DbContextOptions<NewsApiDbContext> options) : base(options)
        {
          
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            modelBuilder.Entity<User>().HasData(new User { Id = 1, FirstName = "obada", LastName = "halabi", ProfilePicture = "https://www.bing.com/th?id=OIP.RYDAyx95XZfKlV4Utf8Z7QHaEK&w=333&h=187&c=8&rs=1&qlt=90&o=6&pid=3.1&rm=2", Email = "obada@gmail.com", Password = "12345", DisplayName = "obada" },
                                                 new User { Id = 2, FirstName = "fadi", LastName = "halabi", ProfilePicture = "https://www.bing.com/th?id=OIP.RYDAyx95XZfKlV4Utf8Z7QHaEK&w=333&h=187&c=8&rs=1&qlt=90&o=6&pid=3.1&rm=2", Email = "fadi@gmail.com", Password = "123456", DisplayName = "fadi" },
                                                 new User { Id = 3, FirstName = "taher", LastName = "halabi", ProfilePicture = "https://www.bing.com/th?id=OIP.frAlEuXSfGFRLcBxzVRY1AHaER&w=329&h=189&c=8&rs=1&qlt=90&o=6&pid=3.1&rm=2", Email = "taher@gmail.com", Password = "12345", DisplayName = "taher" }
                                               
                                                     );

            modelBuilder.Entity<Author>().HasData(new Author { Id = 1, DisplayName = "ali", Email = "ali@gmail.com", Password = "123", Bio = "aaaaa", ProfilePicture = "https://tse4.mm.bing.net/th/id/OIP.y2TOfKrvLx09_tbuortEygHaEG?w=331&h=183&c=7&r=0&o=5&pid=1.7" },
                                                  new Author { Id = 2, DisplayName = "omar", Email = "omar@gmail.com", Password = "145", Bio = "ooooo", ProfilePicture = "https://tse4.mm.bing.net/th/id/OIP.1FMDAFhu9UEmpewQZBWfqgHaEK?w=326&h=183&c=7&r=0&o=5&pid=1.7" },
                                                  new Author { Id = 3, DisplayName = "ahmad", Email = "ahmad@gmail.com", Password = "165", Bio = "hhhhh", ProfilePicture = "https://tse1.mm.bing.net/th/id/OIP.U8tBnyvXfaWfsx3Q-cIXUAHaHa?w=180&h=180&c=7&r=0&o=5&pid=1.7" }
                                                   
                                                      );
                                 

            modelBuilder.Entity<Category>().HasData(new Category {Id=1,CategoryName= "sport" },
                                                    new Category { Id = 2, CategoryName = "Political" },
                                                    new Category { Id = 3, CategoryName = "Military" });

            modelBuilder.Entity<Article>().HasData(new Article { Id = 1, AuthorId = 1, CategoryId = 1, ViewCount = 0, Title = "فلسطين.. عشرات الإصابات بنابلس وتشييع شهيد بأريحا واقتحام يهودي للأقصى", Content = "أصيب أكثر من 200 فلسطيني في مواجهات بنابلس مع قوات الاحتلال الإسرائيلي، بالتزامن مع تشييع شهيد بأريحا، وبعد ساعات من اقتحام مئات المستوطنين للمسجد الأقصى المبارك في القدس." },
                                                   new Article { Id = 2, AuthorId = 2, CategoryId = 2, ViewCount = 0, Title = "الاحتلال يعتدي على فلسطينيين ويمنعهم من دخول الأقصى", Content = "اقتحم مستوطنون باحات المسجد الأقصى صباح اليوم الأحد بحماية قوات الاحتلال الإسرائيلي التي اعتدت على شبان فلسطينيين عند باب الأسباط ومنعتهم بالقوة من دخول المسجد الأقصى لأداء صلاة فجر اليوم، كما منعت عشرات النساء من الدخول." },
                                                   new Article { Id = 3, AuthorId = 3, CategoryId = 3, ViewCount = 0, Title = "حوادث اغتيال العسكريين السودانيين", Content = "منذ بداية الشهر الجاري هجمات على ضباط في الجيش والقوات الأمنية أدت إلى مقتل ضابط في الجيش وآخر في الشرطة وثالث في الدعم السريع ونهب مركبات عسكرية،" }
                                                   );

            modelBuilder.Entity<Like>().HasData(new Like { Id = 1, ArticleId = 1, UserId = 1 },
                                                 new Like { Id = 2, ArticleId = 2, UserId = 2 },
                                                 new Like { Id = 3, ArticleId = 3, UserId = 3 });

            modelBuilder.Entity<Comment>().HasData(new Comment { Id = 1, ArticleId = 1, UserId = 1, CommentText = "wwwww" },
                                                   new Comment { Id = 2, ArticleId = 2, UserId = 2, CommentText = "sssss" },
                                                   new Comment { Id = 3, ArticleId = 3, UserId = 3, CommentText = "xxxxxxx" });

            modelBuilder.Entity<Image>().HasData(new Image { Id = 1, ArticleId = 1, ImageDescription = "no", ImageUrl = "https://tse4.mm.bing.net/th/id/OIP.P-lDxR5o6Hatd2C5RBKukAHaEO?w=263&h=180&c=7&r=0&o=5&pid=1.7", },
                                                 new Image { Id = 2, ArticleId = 2, ImageDescription = "no", ImageUrl = "https://tse2.mm.bing.net/th/id/OIP.W2fvNzcjgTB7zbO9NDRXSwHaFL?w=212&h=180&c=7&r=0&o=5&pid=1.7", },
                                                 new Image { Id = 3, ArticleId = 3, ImageDescription = "no", ImageUrl = "https://tse4.mm.bing.net/th/id/OIP.5zlHy1zk4adkwBWLRxVUqgHaFE?w=233&h=180&c=7&r=0&o=5&pid=1.7", });

            modelBuilder.Entity<Log>().HasData(new Log { Id = 1, AuthorId = 1, UserId = null, logLevel = (NewsApiDomin.Enum.LogLevel)LogLevel.Information, Content = "add", },
                                               new Log { Id = 2, AuthorId = 1, UserId = null, logLevel = (NewsApiDomin.Enum.LogLevel)LogLevel.Information, Content = "delete", },
                                               new Log { Id = 3, AuthorId = null, UserId = 1, logLevel = (NewsApiDomin.Enum.LogLevel)LogLevel.Information, Content = "update", });
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken>  RefreshTokens { get; set; }
    }
}