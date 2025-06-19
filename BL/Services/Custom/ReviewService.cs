using BL.Contracts.IMapper;
using BL.Contracts.Services.Items;
using BL.Services.Generic;
using DAL.Contracts.Repositories.Generic;
using Domains.Entities;
using Shared.DTO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Custom
{
    public class ReviewService : BaseService<TbReview , ReviewDto>, IReviewService
    {
        private readonly ITableRepository<TbReview> _baseTableRepository;
        private readonly IBaseMapper _mapper;
        public ReviewService(ITableRepository<TbReview> baseTableRepository, IBaseMapper mapper) : base(baseTableRepository, mapper)
        {
            _baseTableRepository = baseTableRepository;
            _mapper = mapper;
        }
        }

    }
