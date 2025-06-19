using Domains.Entities.Base;
using Domains.Identity;

namespace Domains.Entities
{
    public class TbFavoriteBook : BaseEntity    
    {
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public Guid BookId { get; set; }
        public TbBook? Book { get; set; }

    }
}
