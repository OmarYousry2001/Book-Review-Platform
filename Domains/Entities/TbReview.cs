using Domains.Entities.Base;
using Domains.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities
{
    public class TbReview : BaseEntity
    {

        [Required]
        public Guid BookId { get; set; }
        public TbBook Book { get; set; }

        [Required]
        public string UserId { get; set; }  = null!;
        public ApplicationUser User { get; set; }

        [Required]
        [Range(1, 5)]
        public EnRating Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; }

    }
}
