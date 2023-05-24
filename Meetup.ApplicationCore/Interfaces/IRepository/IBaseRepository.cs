namespace Meetup.ApplicationCore.Interfaces.IRepository
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        public Task<IEnumerable<T>> GetAllAsync();

        public Task<T?> GetByIdAsync(int id);

        public Task CreateAsync(T entity);

        public Task UpdateAsync(T entity);

        public Task DeleteAsync(T entity);

        public Task<bool> EntityExists(int Id);

        public Task<bool> SaveChangesAsync();
    }
}
