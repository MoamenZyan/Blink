
// ** Repository is just a class that works directly with database **
// Interface for every repository
public interface IRepository<T>
{
    // Every repository class should have these methods
    Task<T?> AddAsync(T Entity);
    Task<T?> GetByIdAsync(int id);
    Task<List<T>?> GetAllAsync();
    IEnumerable<T>? Filter(Func<T, bool> func);
    Task<bool> Update(T Entity);
    Task<bool> Delete(T entities);
}
