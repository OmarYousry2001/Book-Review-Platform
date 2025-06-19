using Domains.Entities;
using Domains.Entities.Base;
using Domains.Identity;
using Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Entities
{
    public class ReviewDto : BaseDto
    {

        public Guid BookId { get; set; }

        public string? UserId { get; set; }

        [Required]
        [Range(1, 5)]
        public EnRating Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; }
    }
}
