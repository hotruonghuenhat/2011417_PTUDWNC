using Azure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;

namespace TatBlog.Data.Seeders {
    public class DataSeeder : IDataSeeder {
        private readonly BlogDbContext _dbContext;

        public DataSeeder(BlogDbContext dbContext) {
            _dbContext = dbContext;
        }

        public void Initialize() {
            _dbContext.Database.EnsureCreated();

            if (_dbContext.Posts.Any()) return;

            var authors = AddAuthors();
            var categories = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, categories, tags);
        }
        private IList<Author> AddAuthors() {
            {

                var authors = new List<Author>()
            {
                new()
                {
                    FullName ="Jason Mouth",
                    UrlSlug ="jason-mouth",
                    Email ="json@gmail.com",
                    JoinedDate = new DateTime(2022, 10, 21)
                },
                new()
                {
                    FullName ="Jessica Wonder",
                    UrlSlug ="jessica-wonder",
                    Email ="jessica665@gmotip.com",
                    JoinedDate = new DateTime(2022, 4, 19)
                },
                new()
                {
                    FullName ="Kathy Smith",
                    UrlSlug ="Kathy-Smith",
                    Email ="KathySmith@gmotip.com",
                    JoinedDate = new DateTime(2010, 9, 06)
                },
                new() {
                    FullName = "Leanne Graham",
                    UrlSlug = "leanne-graham",
                    Email = "leanne@gmail.com",
                    JoinedDate = new DateTime(2022, 12, 1)
                },
                new() {
                    FullName = "Ervin Howell",
                    UrlSlug = "ervin-howell",
                    Email = "ervin@gmail.com",
                    JoinedDate = new DateTime(2023, 1, 22)
                },
                new() {
                    FullName = "Clementine Bauch",
                    UrlSlug = "clementine-bauch",
                    Email = "clementine@gmail.com",
                    JoinedDate = new DateTime(2022, 11, 23)
                },
                new() {
                    FullName = "Patricia Lebsack",
                    UrlSlug = "patricia-lebsack",
                    Email = "patricia@gmail.com",
                    JoinedDate = new DateTime(2021, 7, 8)
                },
                new() {
                    FullName = "Chelsey Dietrich",
                    UrlSlug = "chelsey-dietrich",
                    Email = "chelsey@gmail.com",
                    JoinedDate = new DateTime(2022, 3, 14)
                }
};
                _dbContext.Authors.AddRange(authors);
                _dbContext.SaveChanges();

                return authors;
            }
        }
        private IList<Category> AddCategories() {
            var categories = new List<Category>()
            {
               new() {Name =".NET Core", Description =".NET Core", UrlSlug ="aspnet-core", ShowOnMenu =true},
               new() {Name = "Architecture", Description = "Architecture", UrlSlug = "architecture",ShowOnMenu =true },
               new() {Name = "Messaging", Description = "Messaging", UrlSlug = "messaging",ShowOnMenu =true },
               new() {Name ="OOP", Description ="Object-Oriented Program", UrlSlug ="object-oriented-program",ShowOnMenu =true},
               new() {Name ="Design Patterns", Description ="Design Patterns", UrlSlug ="design-patterns",ShowOnMenu =true},
               new() {Name = "React", Description = "React", UrlSlug = "react"},
               new() {Name = "Angular", Description = "Angular", UrlSlug = "angular"},
               new() {Name = "Vue.js", Description = "Vue.js", UrlSlug = "vue-js"},
               new() {Name = "Next.js", Description = "Next.js", UrlSlug = "next-js"},
               new() {Name = "Node.js", Description = "Node.js", UrlSlug = "node-js"},
               new() {Name = "Golang", Description = "Golang", UrlSlug = "golang"},
               new() {Name = "Three.js", Description = "Three.js", UrlSlug = "three-js"},
               new() {Name = "PHP", Description = "PHP", UrlSlug = "php"},
               new() {Name = "Laravel", Description = "Laravel", UrlSlug = "laravel"},
               new() {Name = "Svelte", Description = "Svelte", UrlSlug = "svelte"},
               new() {Name ="Domain Driven Design", Description ="Domain Driven Design", UrlSlug ="Domain-Driven-Design",ShowOnMenu =true },
               new() {Name ="Programming languages", Description ="Programming languages", UrlSlug ="Programming-languages",ShowOnMenu =true },
               new() {Name ="Practices", Description ="Practices", UrlSlug ="Practices",ShowOnMenu =true }
            };
            _dbContext.AddRange(categories);
            _dbContext.SaveChanges();
            return categories;
        }

        private IList<Tag> AddTags() {

            var tags = new List<Tag>()
        {
            new() {Name = "Google", Description ="Google applications", UrlSlug ="google-applications"},
            new() {Name = "ASP.NET MVC", Description = "ASP.NET MVC", UrlSlug = "asp.net-mvc"},
            new() {Name = "Razor Page", Description = "razor page", UrlSlug = "razor-page"},
            new() {Name = "Deep Learning", Description ="deep learning", UrlSlug ="deep-learning"},
            new() {Name = "Neural Network", Description ="neural network", UrlSlug ="neural-network"},
            new() {Name = "Front-End Applications", Description = "Front-End Applications", UrlSlug = "font-end-application"},
            new() {Name = "Visual Studio", Description = "Visual Studio", UrlSlug = "visual-studio"},
            new() {Name = "SQL Server", Description = "SQL Server", UrlSlug = "sql-server"},
            new() {Name = "Git", Description = "Git", UrlSlug = "git"},
            new() {Name = "Entity Framework Core", Description = "EF Core", UrlSlug = "entity-framework-core"},
            new() {Name = ".NET Framework", Description = ".NET Framework", UrlSlug = "net-framework"},
            new() {Name = "ASP.NET Core", Description = "ASP.NET Core", UrlSlug = "aspnet-core"},
            new() {Name = "Postman", Description = "Postman", UrlSlug = "postman"},
            new() {Name = "ChatGPT", Description = "Chat GPT", UrlSlug = "chat-gpt"},
            new() {Name = "Data cleansing", Description = "Data cleaning", UrlSlug = "data-cleansing"},
            new() {Name = "Fetch API", Description = "Fetch API", UrlSlug = "fetch-api"},
            new() {Name = "Microsoft", Description = "Microsoft", UrlSlug = "microsoft"},
            new() {Name = "Microservices", Description = "Microservices", UrlSlug = "microservices"},
            new() {Name = "Web API Security", Description = "Web API Security", UrlSlug = "web-api-security"}

        };
            _dbContext.AddRange(tags);
            _dbContext.SaveChanges();

            return tags;
        }

        private IList<Post> AddPosts(IList<Author> authors, IList<Category> categories, IList<Tag> tags) {
            var posts = new List<Post>()
        {
            new()
            {
                Title = "ASP.NET Core Diagnostic Scenarios",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[0],
                ViewCount = 10,
                Category = categories[0],
                Tags = new List<Tag>()
                { tags[1] }
            },
            new()
            {
                Title = "JWT creation and validation in Python using Authlib",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 8, 25, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[0],
                ViewCount = 30,
                Category = categories[5],
                Tags = new List<Tag>()
                { tags[3] }
            },
            new()
            {
                Title = "6 Productivity Shortcuts on Windows 10& 11",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 9, 25, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[0],
                ViewCount = 14,
                Category = categories[3],
                Tags = new List<Tag>()
                { tags[3] }
            },
            new()
            {
                Title = "Azure Virtual Machines vs Aoo Services",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 7, 9, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[0],
                ViewCount = 6,
                Category = categories[6],
                Tags = new List<Tag>()
                { tags[3] }
            },
            new()
            {
                Title = "Array or object Json deserialization",
                ShortDescription = "David and friends has a great repos " ,
                Description = "Here's a few great DON'T and DO examples ",
                Meta = "David and friends has a great repository filled ",
                UrlSlug ="aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime (2022, 7, 9, 10, 20, 0),
                ModifiedDate = null,
                Author= authors[0],
                ViewCount = 19,
                Category = categories[6],
                Tags = new List<Tag>()
                { tags[2] }
            }
        };

            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();
            return posts;

        }
    }
}



