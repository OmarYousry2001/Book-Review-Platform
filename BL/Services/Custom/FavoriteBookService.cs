using BL.Contracts.IMapper;
using BL.Contracts.Services.Items;
using BL.Services.Generic;
using DAL.Contracts.Repositories.Generic;
using Domains.Entities;
using Shared.DTO.Entities;

namespace BL.Services
{
    public class FavoriteBookService : BaseService<TbFavoriteBook, FavoriteBookDto>, IFavoriteBookService
    {
        private readonly ITableRepository<TbFavoriteBook> _baseTableRepository;
        private readonly IBaseMapper _mapper;
        public FavoriteBookService(ITableRepository<TbFavoriteBook> baseTableRepository, IBaseMapper mapper) : base(baseTableRepository, mapper)
        {
            _baseTableRepository = baseTableRepository;
            _mapper = mapper;
        }
        public override bool Delete(Guid id, Guid userId)
        {
            return _baseTableRepository.Remove(id);
        }

        public bool MarkAsFavorite(Guid bookId, Guid userId)
        {
     
            var entity = _baseTableRepository.Find(f =>
            f.BookId == bookId &&
            f.UserId == userId.ToString());
            if(entity != null )
            {
                if(entity.CurrentState == 1)
                   return _baseTableRepository.UpdateCurrentState(entity.Id, userId, 0);
                else
                    return _baseTableRepository.UpdateCurrentState(entity.Id, userId, 1);
            }
            else
                return _baseTableRepository.Save(entity, userId);
  
        }

    }
}
