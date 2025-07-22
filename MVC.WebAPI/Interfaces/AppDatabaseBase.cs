using MVC.Services.ApplicationDBContextService;

namespace MVC.WebAPI.Interfaces
{
    public abstract class AppDatabaseBase
    {
        private readonly AppDbContext _context;
        public AppDatabaseBase(AppDbContext context)
        {
            _context = context;
        }
        public AppDbContext GetDBContext()
        {
            return _context;
        }
    }
}
