using Resources;
using Resources.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Views
{
    public class VwBook
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public DateTime PublishDate { get; set; }
        public string ImagePath { get; set; }
        public string CategoryTitleAr { get; set; }
        public string CategoryTitleEn { get; set; }
        public string AuthorNameAr { get; set; }
        public string AuthorNameEn { get; set; }

    }
}
