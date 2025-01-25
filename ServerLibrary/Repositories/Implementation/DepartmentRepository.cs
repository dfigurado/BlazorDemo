using BaseLibrary.Entities;
using BaseLibrary.Response;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Persistence.Context;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementation
{
    public class DepartmentRepository(AppDbContext appDbContext) : BaseRepository, IGenericRepositoryInterface<Department>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var result = await appDbContext.Departments.FindAsync(id);
            if (result is null) return NotFound();

            appDbContext.Departments.Remove(result);
            await Commit();
            return Success();
        }

        public async Task<List<Department>> GetAll()
        {
            var result = await appDbContext.Departments.ToListAsync();
            return result;
        }

        public async Task<Department> GetById(int id)
        {
            var result = await appDbContext.Departments.FindAsync(id);
            return result!;
        }

        public async Task<GeneralResponse> Insert(Department item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false, "Department already exists");
            appDbContext.Departments.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Department item)
        {
            var result = await appDbContext.Departments.FindAsync(item.Id);
            if (result is null) return NotFound();
            result.Name = item.Name;
            await Commit();
            return Success();
        }

        private async Task Commit() => await appDbContext.SaveChangesAsync();

        private async Task<bool> CheckName(string name)
        {
            var item = await appDbContext.Departments.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
            return item is null;
        }
    }
}
