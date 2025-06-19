using BL.Contracts.IMapper;
using BL.Contracts.Services.Generic;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Generic;
using Domains.Entities.Base;

namespace BL.Services.Generic
{
    public abstract class BaseService<TS, TD> : IBaseService<TS, TD> where TS : BaseEntity
    {
        private readonly ITableRepository<TS> _baseRepository;
        private readonly IBaseMapper _mapper;
        public BaseService(ITableRepository<TS> baseRepository, IBaseMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }

        public virtual TD FindById(Guid Id)
        {
            var entity = _baseRepository.FindById(Id);
            var dto = _mapper.MapModel<TS, TD>(entity);
            return dto;
        }

        public IEnumerable<TD> GetAll()
        {
            var entitiesList = _baseRepository.GetAll();
            var dtoList = _mapper.MapList<TS, TD>(entitiesList);
            return dtoList;
        }

        public virtual bool Save(TD dto, Guid userId)
        {
            var entity = _mapper.MapModel<TD, TS>(dto);
            return _baseRepository.Save(entity, userId);
        }

        public bool Create(TD dto, Guid creatorId)
        {
            var entity = _mapper.MapModel<TD, TS>(dto);
            return _baseRepository.Create(entity, creatorId, out _);
        }

        public bool Update(TD dto, Guid updaterId)
        {
            var entity = _mapper.MapModel<TD, TS>(dto);
            return _baseRepository.Update(entity, updaterId, out _);
        }

        public virtual bool Delete(Guid id, Guid userId)
        {
          return _baseRepository.UpdateCurrentState(id , userId);
        }
    }
}
