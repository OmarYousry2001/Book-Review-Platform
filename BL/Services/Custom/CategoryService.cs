using BL.Contracts.IMapper;
using BL.Contracts.Services.Items;
using BL.Services.Generic;
using DAL.Contracts.Repositories.Generic;
using Domains.Entities;
using Shared.DTO.Entities;

namespace BL.Services
{
    public class CategoryService : BaseService<TbCategory , CategoryDto>, ICategoryService
    {
        private readonly ITableRepository<TbCategory > _baseTableRepository;
        private readonly IBaseMapper _mapper;
        public CategoryService(ITableRepository<TbCategory > baseTableRepository, IBaseMapper mapper) : base(baseTableRepository, mapper)
        {
            _baseTableRepository = baseTableRepository;
            _mapper = mapper;
        }
  
    }
}
