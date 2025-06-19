using Domains.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities;

public class TbSettings : BaseEntity
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
