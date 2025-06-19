using Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Entities
{
    public class AuthorDto : BaseDto
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

    }
}
