namespace Cobble.Abstractions
{
    public interface IFileStore<T>
    {
        public abstract Task<T?> LoadAsync(string path);
        public abstract Task<bool> SaveAsync(string path,T content);
    }
}
