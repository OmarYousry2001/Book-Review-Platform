using BL.Contracts.Services.Generic;
using DAL.Models;
using Domains.Entities;
using Shared.DTO.Entities;
using Shared.DTO.Views;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Services.Items
{
    public interface IBookService : IBaseService<TbBook, BookDto>
    {
        public  new Task<bool> Save(BookDto dto, Guid userId);
        public IEnumerable<BookViewDto> GetBookViews();
        public PaginatedDataModel<BookViewDto> GetPage(BaseSearchCriteriaModel criteriaModel);
    }
}
