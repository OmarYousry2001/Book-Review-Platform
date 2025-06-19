using Domains.Entities.Base;
using Domains.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities
{
    public class TbReport : BaseEntity
    {
        [Required]
        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public Guid BookId { get; set; }
        public TbBook? Book { get; set; }
    }
}
