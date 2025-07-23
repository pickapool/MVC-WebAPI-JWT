namespace MVC.Services
{
    public abstract class MethodServiceBase
    {
        public abstract Task<T> GetAll<T>();
    }
}
