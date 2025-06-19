using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Services.Items;
using BL.GeneralService.CMS;
using BL.Services.Generic;
using DAL.Contracts.Repositories.Generic;
using DAL.Models;
using DAL.Repositories.Generic;
using Domains.Entities;
using Domains.Views;
using Shared.DTO.Entities;
using Shared.DTO.Views;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Services
{
    public class BookService : BaseService<TbBook  , BookDto>, IBookService
    {
        private readonly ITableRepository<TbBook> _baseTableRepository;
        private readonly IRepository<VwBook> _baseRepository;

        private readonly IBaseMapper _mapper;
        private readonly IFileUploadService _fileUploadService;
        private readonly IImageProcessingService _imageProcessingService;
        public BookService(ITableRepository<TbBook > baseTableRepository, IRepository<VwBook> baseRepository,IFileUploadService fileUploadService,
            IImageProcessingService imageProcessingService, IBaseMapper mapper) : base(baseTableRepository, mapper)
        {
            _baseTableRepository = baseTableRepository;
            _baseRepository = baseRepository;
            _fileUploadService = fileUploadService;
            _imageProcessingService = imageProcessingService;
            _mapper = mapper;
        }
            
        public IEnumerable<BookViewDto> GetBookViews()
        {
            var entitiesList = _baseRepository.GetAll();
            var dtoList = _mapper.MapList<VwBook, BookViewDto>(entitiesList);
            return dtoList;
        }
        public PaginatedDataModel<BookViewDto> GetPage(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), "Page number must be greater than zero.");

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), "Page size must be between 1 and 100.");


            
            // Default filter returns all
            Expression<Func<VwBook, bool>> filter = x => true;

            // Apply search term if provided
            if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
            {
                string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
                         filter = x =>
                             ((x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm)) ||
                             (x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm)));
            }

            var entitiesList = _baseRepository.GetPage(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter);

            var dtoList = _mapper.MapList<VwBook, BookViewDto>(entitiesList.Items);

            return new PaginatedDataModel<BookViewDto>(dtoList, entitiesList.TotalRecords);
        }





        public async new Task<bool> Save(BookDto dto, Guid userId)
        {

            if (dto.Image != null)
            {
                var bytes = await _fileUploadService.GetFileBytesAsync(dto.Image);
                dto.ImagePath = await ProcessAndUploadImage(bytes, dto.ImagePath, "Images");
            }

            return base.Save(dto, userId);
        }
        private async Task<string> ProcessAndUploadImage(byte[] imageBytes, string? oldImagePath, string nameFolder)
        {
            //  Resize and convert image to WebP
            var resized = _imageProcessingService.ResizeImage(imageBytes, 1024, 1024);
            var webp = _imageProcessingService.ConvertToWebP(resized);

            //  Extract old file name (if any)
            var oldFileName = !string.IsNullOrWhiteSpace(oldImagePath) ? Path.GetFileName(oldImagePath) : null;

            //  Upload new image and update path
            return await _fileUploadService.UploadFileAsync(webp, nameFolder, oldFileName);
        }

    }
}
