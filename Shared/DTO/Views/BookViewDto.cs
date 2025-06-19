using Resources;
using Resources.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTO.Views
{
    public class BookViewDto
    {

        public Guid Id { get; set; }
        [JsonIgnore]
        public string TitleAr { get; set; }
        [JsonIgnore]
        public string TitleEn { get; set; }
        public string Title
        => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;

        [JsonIgnore]
        public string DescriptionAr { get; set; }
        [JsonIgnore]
        public string DescriptionEn { get; set; }
        public string Description
       => ResourceManager.CurrentLanguage == Language.Arabic ? DescriptionAr : DescriptionEn;
        public DateTime PublishDate { get; set; }

        public string ImagePath { get; set; }
        [JsonIgnore]

        public string CategoryTitleAr { get; set; }
        [JsonIgnore]

        public string CategoryTitleEn { get; set; }

        public string CategoryTitle
        => ResourceManager.CurrentLanguage == Language.Arabic ? CategoryTitleAr : CategoryTitleEn;
        [JsonIgnore]

        public string AuthorNameAr { get; set; }
        [JsonIgnore]

        public string AuthorNameEn { get; set; }

        public string AuthorName
        => ResourceManager.CurrentLanguage == Language.Arabic ? AuthorNameAr : AuthorNameEn;
    }
}
