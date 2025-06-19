using BL.Contracts.Services.Generic;
using Domains.Entities;
using Shared.DTO.Entities;

namespace BL.Contracts.Services.Items
{
    public interface IFavoriteBookService : IBaseService<TbFavoriteBook, FavoriteBookDto>
    {

        public bool MarkAsFavorite(Guid bookId, Guid userId);
    }
}
