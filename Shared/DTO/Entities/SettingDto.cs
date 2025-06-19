using Domains.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Entities
{
    public class SettingDto : BaseEntity
    {
        [MaxLength(100)]
        public string WebsiteNameAr { get; set; } = null!;
        [MaxLength(100)]
        public string WebsiteNameEn { get; set; } = null!;
        [MaxLength(100)]
        public string Logo { get; set; } = null!;
        [MaxLength(100)]
        public string FacebookLink { get; set; } = null!;
        [MaxLength(100)]
        public string TwitterLink { get; set; } = null!;
        [MaxLength(100)]
        public string InstagramLink { get; set; } = null!;
        [MaxLength(100)]
        public string YoutubeLink { get; set; } = null!;
        [MaxLength(100)]
        public string AddressAr { get; set; } = null!;
        [MaxLength(100)]
        public string AddressEn { get; set; } = null!;
        [MaxLength(100)]
        public string ContactNumber { get; set; } = null!;
        [MaxLength(100)]
        public string Fax { get; set; } = null!;
        [MaxLength(100)]
        public string Email { get; set; } = null!;

    }
}
