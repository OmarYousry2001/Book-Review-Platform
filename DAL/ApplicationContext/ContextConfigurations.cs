using Domains.Entities;
using Domains.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ApplicationContext
{
    public class ContextConfigurations
    {
        private static readonly string seedAdminEmail = "admin@gmail.com";
        private static readonly string seedAdminPassword = "123456";

        public static async Task SeedDataAsync(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Seed user first to get admin user ID
            var adminUserId = await SeedUserAsync(userManager, roleManager);

            // Seed E-commerce data
            await SeedECommerceDataAsync(context, adminUserId);
        }
        private static async Task SeedECommerceDataAsync(ApplicationDbContext context, Guid adminUserId)
        {
            // 1. Seed TbCategory
            if (!context.TbCategory.Any())
            {
                var categories = new List<TbCategory>
    {
        new TbCategory
        {
            Id = Guid.NewGuid(),
            TitleAr = "روايات",
            TitleEn = "Novels",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbCategory
        {
            Id = Guid.NewGuid(),
            TitleAr = "تاريخ",
            TitleEn = "History",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbCategory
        {
            Id = Guid.NewGuid(),
            TitleAr = "تكنولوجيا",
            TitleEn = "Technology",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbCategory
        {
            Id = Guid.NewGuid(),
            TitleAr = "تطوير الذات",
            TitleEn = "Self-Development",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbCategory
        {
            Id = Guid.NewGuid(),
            TitleAr = "علوم",
            TitleEn = "Science",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        }
    };

                await context.TbCategory.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
            // 2. Seed TbAuthor
            if (!context.TbAuthor.Any())
            {
                var authors = new List<TbAuthor>
    {
        new TbAuthor
        {
            Id = Guid.NewGuid(),
            NameAr = "نجيب محفوظ",
            NameEn = "Naguib Mahfouz",
            NationalityAr = "مصري",
            NationalityEn = "Egyptian",
            BioAr = "كاتب مصري حائز على جائزة نوبل، من أبرز الروائيين العرب.",
            BioEn = "An Egyptian writer who won the Nobel Prize, considered one of the greatest Arabic novelists.",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbAuthor
        {
            Id = Guid.NewGuid(),
            NameAr = "جبران خليل جبران",
            NameEn = "Gibran Khalil Gibran",
            NationalityAr = "لبناني",
            NationalityEn = "Lebanese",
            BioAr = "كاتب وشاعر وفنان لبناني أمريكي، أشهر أعماله كتاب النبي.",
            BioEn = "A Lebanese-American poet, writer, and artist, best known for his book 'The Prophet'.",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbAuthor
        {
            Id = Guid.NewGuid(),
            NameAr = "أحمد خالد توفيق",
            NameEn = "Ahmed Khaled Tawfik",
            NationalityAr = "مصري",
            NationalityEn = "Egyptian",
            BioAr = "رائد أدب الرعب والخيال العلمي في العالم العربي.",
            BioEn = "Pioneer of horror and science fiction literature in the Arab world.",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbAuthor
        {
            Id = Guid.NewGuid(),
            NameAr = "مصطفى محمود",
            NameEn = "Mostafa Mahmoud",
            NationalityAr = "مصري",
            NationalityEn = "Egyptian",
            BioAr = "كاتب وفيلسوف وطبيب مصري، صاحب برنامج العلم والإيمان.",
            BioEn = "Egyptian writer, philosopher, and doctor, known for his TV program 'Science and Faith'.",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbAuthor
        {
            Id = Guid.NewGuid(),
            NameAr = "توفيق الحكيم",
            NameEn = "Tawfiq Al-Hakim",
            NationalityAr = "مصري",
            NationalityEn = "Egyptian",
            BioAr = "من رواد المسرح والرواية في الأدب العربي الحديث.",
            BioEn = "One of the pioneers of modern Arabic theater and literature.",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbAuthor
        {
            Id = Guid.NewGuid(),
            NameAr = "إبراهيم الفقي",
            NameEn = "Ibrahim El-Fiky",
            NationalityAr = "مصري",
            NationalityEn = "Egyptian",
            BioAr = "خبير في التنمية البشرية والبرمجة اللغوية العصبية.",
            BioEn = "Expert in human development and NLP (Neuro-Linguistic Programming).",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        }
    };

                await context.TbAuthor.AddRangeAsync(authors);
                await context.SaveChangesAsync();
            }
            // 3. Seed TbBook
            if (!context.TbBook.Any())
            {

                var naguibMahfouz = await context.TbAuthor.FirstOrDefaultAsync(i => i.NameEn == "Naguib Mahfouz");
                var mostafaMahmoud = await context.TbAuthor.FirstOrDefaultAsync(i => i.NameEn == "Mostafa Mahmoud");

                var novels = await context.TbCategory.FirstOrDefaultAsync(i => i.TitleEn == "Novels");
                var history = await context.TbCategory.FirstOrDefaultAsync(i => i.TitleEn == "History");

                var books = new List<TbBook>
    {
        new TbBook
        {
            Id = Guid.NewGuid(),
            TitleAr = "أولاد حارتنا",
            TitleEn = "Children of the Alley",
            DescriptionAr = "رواية رمزية من تأليف نجيب محفوظ تتناول الصراع بين الخير والشر.",
            DescriptionEn = "A symbolic novel by Naguib Mahfouz exploring the struggle between good and evil.",
            CategoryId = novels.Id, // مثال
            AuthorId = naguibMahfouz.Id, // نجيب محفوظ
            PublishDate = new DateTime(1959, 1, 1),
            ImagePath = "images/books/children-of-the-alley.webp",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbBook
        {
            Id = Guid.NewGuid(),
            TitleAr = "النبي",
            TitleEn = "The Prophet",
            DescriptionAr = "أشهر أعمال جبران خليل جبران، يتناول قضايا الحياة والحب والفكر.",
            DescriptionEn = "The most famous work by Gibran Khalil Gibran, discussing life, love, and thought.",
            CategoryId = novels.Id,
            AuthorId = naguibMahfouz.Id, // جبران
            PublishDate = new DateTime(1923, 1, 1),
            ImagePath = "images/books/the-prophet.webp",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbBook
        {
            Id = Guid.NewGuid(),
            TitleAr = "ما وراء الطبيعة - أسطورة رأس ميدوسا",
            TitleEn = "Metaphysics - Medusa's Head",
            DescriptionAr = "جزء من سلسلة ما وراء الطبيعة لأحمد خالد توفيق.",
            DescriptionEn = "Part of the Metaphysics series by Ahmed Khaled Tawfik.",
            CategoryId = novels.Id,
            AuthorId = naguibMahfouz.Id,
            PublishDate = new DateTime(1998, 1, 1),
            ImagePath = "images/books/medusa-head.webp",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbBook
        {
            Id = Guid.NewGuid(),
            TitleAr = "رحلتي من الشك إلى الإيمان",
            TitleEn = "My Journey from Doubt to Faith",
            DescriptionAr = "كتاب فلسفي يشرح رحلة مصطفى محمود نحو الإيمان.",
            DescriptionEn = "A philosophical book explaining Mostafa Mahmoud's journey toward faith.",
            CategoryId = novels.Id,
            AuthorId = naguibMahfouz.Id,
            PublishDate = new DateTime(1970, 1, 1),
            ImagePath = "images/books/doubt-to-faith.webp",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbBook
        {
            Id = Guid.NewGuid(),
            TitleAr = "عودة الروح",
            TitleEn = "Return of the Spirit",
            DescriptionAr = "رواية وطنية اجتماعية من تأليف توفيق الحكيم.",
            DescriptionEn = "A patriotic social novel by Tawfiq Al-Hakim.",
            CategoryId = novels.Id,
            AuthorId = naguibMahfouz.Id,
            PublishDate = new DateTime(1933, 1, 1),
            ImagePath = "images/books/return-of-the-spirit.webp",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbBook
        {
            Id = Guid.NewGuid(),
            TitleAr = "قوة التحكم في الذات",
            TitleEn = "The Power of Self-Control",
            DescriptionAr = "من أشهر كتب إبراهيم الفقي عن تطوير الذات والتحكم بالنفس.",
            DescriptionEn = "One of Ibrahim El-Fiky's famous books on self-development and control.",
            CategoryId = novels.Id,
            AuthorId = mostafaMahmoud.Id,
            PublishDate = new DateTime(2005, 1, 1),
            ImagePath = "images/books/self-control.webp",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbBook
        {
            Id = Guid.NewGuid(),
            TitleAr = "اللص والكلاب",
            TitleEn = "The Thief and the Dogs",
            DescriptionAr = "رواية قصيرة لنجيب محفوظ تعالج موضوعات العدالة والانتقام.",
            DescriptionEn = "A short novel by Naguib Mahfouz dealing with justice and revenge.",
            CategoryId = history.Id,
            AuthorId = mostafaMahmoud.Id,
            PublishDate = new DateTime(1961, 1, 1),
            ImagePath = "images/books/thief-and-dogs.webp",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbBook
        {
            Id = Guid.NewGuid(),
            TitleAr = "خواطر شاب",
            TitleEn = "Thoughts of a Young Man",
            DescriptionAr = "كتاب تطوير ذات يحفز الشباب على النجاح.",
            DescriptionEn = "A motivational book for youth, encouraging personal success.",
            CategoryId = history.Id,
            AuthorId = mostafaMahmoud.Id,
            PublishDate = new DateTime(2010, 1, 1),
            ImagePath = "images/books/young-thoughts.webp",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbBook
        {
            Id = Guid.NewGuid(),
            TitleAr = "عصفور من الشرق",
            TitleEn = "Bird from the East",
            DescriptionAr = "رواية فكرية رومانسية من تأليف توفيق الحكيم.",
            DescriptionEn = "A romantic and philosophical novel by Tawfiq Al-Hakim.",
            CategoryId = history.Id,
            AuthorId = mostafaMahmoud.Id,
            PublishDate = new DateTime(1938, 1, 1),
            ImagePath = "images/books/bird-from-east.webp",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new TbBook
        {
            Id = Guid.NewGuid(),
            TitleAr = "السر",
            TitleEn = "The Secret",
            DescriptionAr = "كتاب تحفيزي مشهور عن قانون الجذب وتحقيق الأحلام.",
            DescriptionEn = "A famous motivational book about the Law of Attraction and achieving dreams.",
            CategoryId = history.Id,
            AuthorId = mostafaMahmoud.Id,
            PublishDate = new DateTime(2006, 1, 1),
            ImagePath = "images/books/the-secret.webp",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        }
    };

                await context.TbBook.AddRangeAsync(books);
                await context.SaveChangesAsync();
            }
            // 4. Seed TbSettings
            if (!context.TbSettings.Any())
            {
                var settings = new TbSettings
                {
                    Id = Guid.NewGuid(),
                    WebsiteNameAr = "منصة مراجعة الكتب",
                    WebsiteNameEn = "Book Review Platform",
                    Logo = "images/logo/logo.png",
                    FacebookLink = "https://facebook.com/yourpage",
                    TwitterLink = "https://twitter.com/yourpage",
                    InstagramLink = "https://instagram.com/yourpage",
                    YoutubeLink = "https://youtube.com/yourchannel",
                    AddressAr = "القاهرة، مصر",
                    AddressEn = "Cairo, Egypt",
                    ContactNumber = "+201234567890",
                    Fax = "+202987654321",
                    Email = "support@bookplatform.com",
                    CurrentState = 1,
                    CreatedBy = adminUserId,
                    CreatedDateUtc = DateTime.UtcNow
                };

                await context.TbSettings.AddAsync(settings);
                await context.SaveChangesAsync();
            }

        }
        private static async Task<Guid> SeedUserAsync(UserManager<ApplicationUser> userManager,
                    RoleManager<IdentityRole> roleManager)
        {
            // Ensure roles exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Reader"))
            {
                await roleManager.CreateAsync(new IdentityRole("Reader"));
            }

            if (!await roleManager.RoleExistsAsync("Writer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Writer"));
            }

            // Ensure admin user exists
            var adminEmail = seedAdminEmail;
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var id = Guid.NewGuid().ToString();
                adminUser = new ApplicationUser
                {
                    Id = id,
                    UserName = adminEmail,
                    City = "Cairo",
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "Eladmin",
                    EmailConfirmed = true,
                    CreatedDateUtc = DateTime.UtcNow
                };
                var result = await userManager.CreateAsync(adminUser, seedAdminPassword);
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Convert adminUser.Id from string to Guid
            return Guid.Parse(adminUser.Id);  // Convert to Guid
        }
    }
}
