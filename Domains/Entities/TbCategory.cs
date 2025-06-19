using Domains.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities
{
    public class TbCategory : BaseEntity
    {

        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = null!;

        public ICollection<TbBook> Books { get; set; }

        public TbCategory()
        {
            Books = new List<TbBook>();
        }
    }
}
