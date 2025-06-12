using Domains.Entities.Base;

namespace Domains.Entities;

public class TbSettings : BaseEntity
{
    public string WebsiteNameAr { get; set; } = null!;
    public string WebsiteNameEn { get; set; } = null!;
    public string Logo { get; set; } = null!;
    public string FacebookLink { get; set; } = null!;
    public string TwitterLink { get; set; } = null!;
    public string InstagramLink { get; set; } = null!;
    public string YoutubeLink { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string ContactNumber { get; set; } = null!;
    public string Fax { get; set; } = null!;
    public string Email { get; set; } = null!;  



}
