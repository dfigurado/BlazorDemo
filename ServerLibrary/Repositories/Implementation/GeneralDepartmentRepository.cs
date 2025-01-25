using BaseLibrary.Entities;
using BaseLibrary.Response;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Persistence.Context;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementation
{
    public class GeneralDepartmentRepository(AppDbContext appDbContext) : BaseRepository, IGenericRepositoryInterface<GeneralDepartment>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var result = await appDbContext.GeneralDepartments.FindAsync(id);
            if (result == null) return NotFound();

            appDbContext.GeneralDepartments.Remove(result);
            await Commit();
            return Success();
        }

        public async Task<List<GeneralDepartment>> GetAll()
        {
            var result = await appDbContext.GeneralDepartments.ToListAsync();
            return result;
        }

        public async Task<GeneralDepartment> GetById(int id)
        {
            var result = await appDbContext.GeneralDepartments.FindAsync(id);
            return result!;
        }

        public async Task<GeneralResponse> Insert(GeneralDepartment item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false, "Department already exists");
            appDbContext.GeneralDepartments.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(GeneralDepartment item)
        {
            var result = await appDbContext.GeneralDepartments.FindAsync(item.Id);
            if (result is null) return NotFound();
            result.Name = item.Name;
            await Commit();
            return Success();
        }

        private async Task Commit() => await appDbContext.SaveChangesAsync();

        private async Task<bool> CheckName(string name)
        {
            var item = await appDbContext.GeneralDepartments.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
            return item is null;
        }
    }
}
