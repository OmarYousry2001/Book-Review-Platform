    using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Generic;
using DAL.Exceptions;
using DAL.Models;
using Domains.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DAL.Repositories.Generic
{
    public class TableRepository<T> : Repository<T>, ITableRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        protected DbSet<T> DbSet => _dbContext.Set<T>();

        public TableRepository(ApplicationDbContext dbContext, ILogger logger) : base(dbContext, logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Database context cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger instance cannot be null.");
        }

        /// <summary>
        /// Retrieves all active entities (CurrentState == 1).
        /// </summary>
        public override IEnumerable<T> GetAll()
        {
            try
            {
                return DbSet.AsNoTracking().Where(e => e.CurrentState == 1).ToList();
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetAll), $"Error occurred while retrieving all active entities of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves entities based on a predicate, filtering only active records.
        /// </summary>
        public override IEnumerable<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                if (predicate == null)
                    return DbSet.AsNoTracking().Where(e => e.CurrentState == 1).ToList();

                return DbSet.Where(predicate)?.Where(e => e.CurrentState == 1).AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                HandleException(nameof(Get), $"Error occurred while filtering active entities of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves paginated data.
        /// </summary>
        public override  PaginatedDataModel<T> GetPage(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            try
            {
                IQueryable<T> query = DbSet.AsNoTracking();

                // Apply the filter if it exists
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                // Apply ordering if provided
                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                // Get the total count before pagination
                int totalCount = query.Count();

                // Apply pagination
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                // Execute the query and return the paginated data
                var data = query.ToList();

                return new PaginatedDataModel<T>(data, totalCount);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(
                    $"Error occurred in {nameof(GetPage)} method for entity type {typeof(T).Name}.",
                    ex,
                    _logger
                );
            }
        }

        /// <summary>
        /// Finds an entity by its ID.
        /// </summary>
        public T FindById(Guid id)
        {
            try
            {
                var data = DbSet.Find(id);
                if (data == null)
                    throw new NotFoundException($"Entity of type {typeof(T).Name} with ID {id} not found.", _logger);

                return data;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(FindById), $"Error occurred while finding an entity of type {typeof(T).Name} with ID {id}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Saves or updates an entity based on its key.
        /// </summary>
        public bool Save(T model, Guid userId)
        {
            try
            {
                if (model.Id == Guid.Empty)
                    return Create(model, userId, out _);
                else
                    return Update(model, userId, out _);
            }
            catch (Exception ex)
            {
                HandleException(nameof(Save), $"Error occurred while saving or updating an entity of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Saves or updates an entity based on its key and outputs the ID of the saved entity.
        /// </summary>
        public bool Save(T model, Guid userId, out Guid id)
        {
            try
            {
                if (model.Id == Guid.Empty)
                    return Create(model, userId, out id);
                else
                    return Update(model, userId, out id);
            }
            catch (Exception ex)
            {
                HandleException(nameof(Save), $"Error occurred while saving or updating an entity of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        public bool Create(T model, Guid creatorId, out Guid id)
        {
            try
            {
                id = Guid.NewGuid();

                model.Id = id;
                model.CreatedDateUtc = DateTime.UtcNow;
                model.CreatedBy = creatorId;
                model.CurrentState = 1;

                DbSet.Add(model);
                return _dbContext.SaveChanges() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                HandleException(nameof(Create), $"Conflict error while creating an entity of type {typeof(T).Name}.", dbEx);
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(Create), $"Error occurred while creating an entity of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        public bool Update(T model, Guid updaterId, out Guid id)
        {
            try
            {
                var existingEntity = DbSet.AsNoTracking().FirstOrDefault(e => e.Id == model.Id);
                id = model.Id;

                if (existingEntity == null)
                    throw new DataAccessException($"Entity with key {id} not found.", _logger);

                model.UpdatedDateUtc = DateTime.UtcNow;
                model.UpdatedBy = updaterId;
                model.CreatedBy = existingEntity.CreatedBy;
                model.CurrentState = existingEntity.CurrentState;
                model.CreatedDateUtc = existingEntity.CreatedDateUtc;

                DbSet.Entry(model).State = EntityState.Modified;

                return _dbContext.SaveChanges() > 0;
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
            {
                HandleException(nameof(Update), $"Concurrency error while updating an entity of type {typeof(T).Name}, ID {model.Id}.", concurrencyEx);
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(Update), $"Error occurred while updating an entity of type {typeof(T).Name}, ID {model.Id}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the CurrentState of an entity.
        /// </summary>
        public bool UpdateCurrentState(Guid entityId, Guid userId,  int newValue = 0)
        {
            try
            {
                var entity = DbSet.Find(entityId);

                if (entity == null)
                    throw new NotFoundException($"Entity of type {typeof(T).Name} with ID {entityId} not found.", _logger);

                entity.CurrentState = newValue;
                entity.UpdatedBy = userId;
                

                DbSet.Update(entity);
                return _dbContext.SaveChanges() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                HandleException(nameof(UpdateCurrentState), $"Database update error while updating CurrentState for entity type {typeof(T).Name}, ID {entityId}.", dbEx);
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(UpdateCurrentState), $"Error occurred while updating CurrentState for entity type {typeof(T).Name}, ID {entityId}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        public bool SaveChange()
        {
            try
            {
                return _dbContext.SaveChanges() > 0;
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
            {
                HandleException(nameof(SaveChange), $"Concurrency error while saving changes for entity type {typeof(T).Name}.", concurrencyEx);
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(SaveChange), $"Error occurred while saving changes for entity type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Adds multiple new entities to the database
        /// </summary>
        public bool AddRange(IEnumerable<T> entities, Guid userId)
        {
            try
            {
                var utcNow = DateTime.UtcNow;

                foreach (var entity in entities)
                {
                    entity.Id = Guid.NewGuid();
                    entity.CreatedDateUtc = utcNow;
                    entity.CreatedBy = userId;
                    entity.CurrentState = 1;
                }

                DbSet.AddRange(entities);
                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                HandleException(nameof(AddRange), $"Error occurred while adding multiple entities of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates multiple existing entities in the database
        /// </summary>
        public bool UpdateRange(IEnumerable<T> entities, Guid userId)
        {
            try
            {
                var utcNow = DateTime.UtcNow;
                var ids = entities.Select(e => e.Id).ToList();
                var existingEntities = DbSet.Where(e => ids.Contains(e.Id)).ToList();

                foreach (var entity in entities)
                {
                    var existingEntity = existingEntities.FirstOrDefault(e => e.Id == entity.Id);
                    if (existingEntity == null)
                        continue;

                    entity.CreatedBy = existingEntity.CreatedBy;
                    entity.CreatedDateUtc = existingEntity.CreatedDateUtc;
                    entity.UpdatedBy = userId;
                    entity.UpdatedDateUtc = utcNow;
                    entity.CurrentState = existingEntity.CurrentState;

                    DbSet.Entry(entity).State = EntityState.Modified;
                }

                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                HandleException(nameof(UpdateRange), $"Error occurred while updating multiple entities of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Saves or updates multiple entities based on their keys and persists the changes to the database.
        /// </summary>
        public bool SaveRange(IEnumerable<T> entities, Guid userId)
        {
            try
            {
                if (!entities.Any())
                    return false;

                foreach (var entity in entities)   
                {
                    if (entity.Id == Guid.Empty)   
                    {
                  
                        Create(entity, userId, out Guid id);
                    }
                    else                         
                    {
                        Update(entity, userId , out Guid id);
                      
                    }
                }

                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                HandleException(nameof(SaveRange),
                    $"Error occurred while SaveRange multiple entities of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Removes existing entities filtered by the given predicate and adds a new set of entities, replacing the old data.
        /// </summary>
        public bool ResetRange(IEnumerable<T> newEntities, Guid userId, Expression<Func<T, bool>> filterForOldEntities)
        {
            try
            {
                var oldEntities = DbSet.Where(filterForOldEntities).ToList();
                if (oldEntities.Any())
                {
                        DbSet.RemoveRange(oldEntities);
                }

                if(newEntities.Any())
                {
          
                    var utcNow = DateTime.UtcNow;
                    foreach (var entity in newEntities)
                    {
                        entity.Id = Guid.NewGuid();
                        entity.CreatedDateUtc = utcNow;
                        entity.CreatedBy = userId;
                        entity.CurrentState = 1;
                    }

                    DbSet.AddRange(newEntities);
                }
            
                var changes = _dbContext.SaveChanges() > 0;

                return changes;
            }
            catch (Exception ex)
            {
                HandleException(nameof(ResetRange), $"Error occurred while replacing entities of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Permanently deletes an entity from the database (Hard Delete).
        /// </summary>
        public bool Remove(Guid id)
        {
            try
            {
                var entity = DbSet.Find(id);
                if (entity == null)
                    throw new NotFoundException($"Entity of type {typeof(T).Name} with ID {id} not found.", _logger);

                DbSet.Remove(entity);
                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                HandleException(nameof(Remove), $"Error occurred while hard-deleting entity of type {typeof(T).Name} with ID {id}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Logs and rethrows exceptions with detailed information.
        /// </summary>
        private void HandleException(string methodName, string message, Exception ex)
        {
            // Log detailed message for analysis
            _logger.Error(ex, $"[{methodName}] {message}");

            // Throw exception with a general message for users
            throw new DataAccessException(message, ex, _logger);
        }
    }
}
