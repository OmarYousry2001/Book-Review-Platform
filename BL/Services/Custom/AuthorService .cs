using BL.Contracts.IMapper;
using BL.Contracts.Services.Items;
using BL.Services.Generic;
using DAL.Contracts.Repositories.Generic;
using Domains.Entities;
using Shared.DTO.Entities;

namespace BL.Services
{
    public class AuthorService : BaseService<TbAuthor, AuthorDto>, IAuthorService
    {
        private readonly ITableRepository<TbAuthor> _baseTableRepository;
        private readonly IBaseMapper _mapper;
        public AuthorService(ITableRepository<TbAuthor> baseTableRepository, IBaseMapper mapper) : base(baseTableRepository, mapper)
        {
            _baseTableRepository = baseTableRepository;
            _mapper = mapper;
        }
  
    }
}
