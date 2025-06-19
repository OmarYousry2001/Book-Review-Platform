using Domains.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities
{
    public class TbBook : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string TitleAr { get; set; }

        [Required]
        [MaxLength(200)]
        public string TitleEn { get; set; }

        [MaxLength(1000)]
        public string? DescriptionAr { get; set; }

        [MaxLength(1000)]
        public string? DescriptionEn { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
        public TbCategory Category { get; set; }

        [Required]
        public Guid AuthorId { get; set; }
        public TbAuthor Author { get; set; }

        public DateTime? PublishDate { get; set; }

        [MaxLength(255)]
        public string? ImagePath { get; set; }

        public ICollection<TbReview> Reviews { get; set; }
        public ICollection<TbFavoriteBook> FavoriteBooks { get; set; }


        public TbBook()
        {
            Reviews = new List<TbReview>();
            FavoriteBooks = new List<TbFavoriteBook>();
        }
    }
}
