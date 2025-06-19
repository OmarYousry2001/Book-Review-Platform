using Api.Controllers.Base;
using BL.Contracts.Services.Items;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTO.Entities;
using Shared.DTO.Views;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.Book
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : BaseController
    {
        private readonly IBookService _bookService;
        private readonly IFavoriteBookService _favoriteBookService;
        private readonly IReportService _reportService;

        public BookController(IBookService bookService
            , IFavoriteBookService favoriteBookService, IReportService reportService,Serilog.ILogger logger)
            : base(logger)
        {
            _bookService = bookService;

            _favoriteBookService = favoriteBookService;
            _reportService= reportService;  
        }

        /// <summary>
        /// Retrieves all books.
        /// </summary>
        /// <remarks>
        /// Returns a list of all available books with details.
        /// </remarks>
        /// <returns>A list of books wrapped in a standardized response model.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var entities = _bookService.GetBookViews();
                if (entities == null || !entities.Any())
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message  = NotifiAndAlertsResources.InvalidInput
                    });

                return Ok(new ResponseModel<IEnumerable<BookViewDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.Successful,
                    Data = entities
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a book by its ID.
        /// </summary>
        /// <remarks>
        /// Provide a valid book ID to retrieve its details.
        /// </remarks>
        /// <returns>The book details wrapped in a standardized response model.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInput
                    });

                var entity = _bookService.FindById(id);
                if (entity == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NotFound
                    });

                return Ok(new ResponseModel<BookDto>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.Successful,
                    Data = entity
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches books with pagination and optional filtering.
        /// </summary>
        /// <remarks>
        /// This endpoint allows filtering and paginating books based on criteria such as page number, page size, and other filters.
        /// </remarks>
        /// <param name="criteria">Search criteria including pagination parameters.</param>
        /// <returns>Paginated list of books.</returns>
        [HttpGet("search")]
        public IActionResult Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            try
            {
                // Validate and set default pagination values if not provided
                criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
                criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

                var result = _bookService.GetPage(criteria);

                if (result == null || !result.Items.Any())
                {
                    return Ok(new ResponseModel<PaginatedDataModel<BookViewDto>>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = result
                    });
                }

                return Ok(new ResponseModel<PaginatedDataModel<BookViewDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Saves a book (create or update).
        /// </summary>
        /// <remarks>
        /// This endpoint is used to create or update a book. It accepts form-data with optional image upload.
        /// </remarks>
        /// <returns>A response indicating the success or failure of the operation.</returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromForm] BookDto BookDto)
        {
            try
            {
                if (BookDto.Image == null && BookDto.ImagePath == null)
                {
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = ValidationResources.RequiredField
                    });
                }

                var success = await _bookService.Save(BookDto, GuidUserId);
                if (!success)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.SaveFailed
                    });

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.SavedSuccessfully
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a book by its ID.
        /// </summary>
        /// <remarks>
        /// Provide a valid book ID as a query parameter to delete the corresponding book.
        /// </remarks>
        /// <returns>Standardized response indicating the outcome.</returns>
        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid Book ID.",
                        StatusCode = StatusCodes.Status400BadRequest

                    });

                var success = _bookService.Delete(id, GuidUserId);
                if (!success)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.DeleteFailed,
                        StatusCode = StatusCodes.Status400BadRequest
                    });

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DeletedSuccessfully,
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Marks a book as favorite for the current user.
        /// </summary>
        /// <remarks>
        /// This endpoint allows the authenticated user to mark a specific book as a favorite.
        /// The book ID must be passed as a query parameter.
        /// </remarks>
        /// <param name="bookId">The ID of the book to be marked as favorite.</param>
        /// <returns>A response indicating success or failure of the operation.</returns>
        [HttpPost("Favorite")]
        public IActionResult Favorite([FromQuery] Guid bookId)
        {
            try
            {
                if (bookId == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = ValidationResources.InvalidData
                    });

                var result = _favoriteBookService.MarkAsFavorite(bookId, GuidUserId);

                if (!result)
                {
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.SaveFailed
                    });
                }

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.SavedSuccessfully
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

    }
}
