using BL.Contracts.IMapper;
using BL.Contracts.Services.Items;
using BL.Services.Generic;
using DAL.Contracts.Repositories.Generic;
using Domains.Entities;
using Domains.Identity;
using Shared.DTO.Entities;
using Shared.DTOs.User;

namespace BL.Services
{
    public class RefreshTokenService : BaseService<TbRefreshToken, RefreshTokenDto>, IRefreshTokenService
    {
        private readonly ITableRepository<TbRefreshToken> _baseTableRepository;
        private readonly IBaseMapper _mapper;
        public RefreshTokenService(ITableRepository<TbRefreshToken> baseTableRepository, IBaseMapper mapper) : base(baseTableRepository, mapper)
        {
            _baseTableRepository = baseTableRepository;
            _mapper = mapper;
        }

        public RefreshTokenDto GetByToken(string token)
        {
            var refreshToken = _baseTableRepository.Get(rt => rt.Token == token).FirstOrDefault();
            return _mapper.MapModel<TbRefreshToken, RefreshTokenDto>(refreshToken);
        }

        public bool Refresh(RefreshTokenDto token)
        {

            var tokenlist = _baseTableRepository.Get(rt => rt.UserId == rt.UserId).ToList();
            foreach (var item in tokenlist)
            {
                _baseTableRepository.UpdateCurrentState(item.Id, Guid.Parse(item.UserId), 0);

            }
            var entity = _mapper.MapModel<RefreshTokenDto, TbRefreshToken>(token);
            _baseTableRepository.Save(entity, Guid.Parse(token.UserId));
            return true;
        }
    }
}
