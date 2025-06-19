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
    public class ReportDto : BaseDto
    {

        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        public string UserId { get; set; }

        public Guid BookId { get; set; }
    }
    
}
