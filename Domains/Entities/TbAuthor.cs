using Domains.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities
{
    public class TbAuthor : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string NameAr { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameEn { get; set; }

        [MaxLength(200)]
        public string? NationalityAr { get; set; }

        [MaxLength(200)]
        public string? NationalityEn { get; set; }


        [MaxLength(1000)]
        public string? BioEn { get; set; }

        [MaxLength(1000)]
        public string? BioAr { get; set; }


        public ICollection<TbBook> Books { get; set; }
    }
}
