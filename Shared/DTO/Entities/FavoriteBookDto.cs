using Domains.Entities;
using Domains.Identity;
using Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Entities
{
    public class FavoriteBookDto : BaseDto
    {
        public string UserId { get; set; }
        public Guid BookId { get; set; }


    }
}
