using BaseLibrary.Entities;
using BaseLibrary.Response;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Persistence.Context;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementation
{
    public class BranchRepository(AppDbContext appDbContext) : BaseRepository, IGenericRepositoryInterface<Branch>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var result = await appDbContext.Branches.FindAsync(id);
            if (result is null) return NotFound();

            appDbContext.Branches.Remove(result);
            await Commit();
            return Success();
        }

        public async Task<List<Branch>> GetAll()
        {
            var result = await appDbContext.Branches.ToListAsync();
            return result;
        }

        public async Task<Branch> GetById(int id)
        {
            var result = await appDbContext.Branches.FindAsync(id);
            return result!;
        }

        public async Task<GeneralResponse> Insert(Branch item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false, "Branch already exists");
            await appDbContext.Branches.AddAsync(item);

            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Branch item)
        {
            var result = await appDbContext.Branches.FindAsync(item.Id);
            if (result is null) return NotFound();
            result.Name = item.Name;
            await Commit();
            return Success();
        }

        private async Task Commit() => await appDbContext.SaveChangesAsync();

        private async Task<bool> CheckName(string name)
        {
            var item = await appDbContext.Branches.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
            return item is null;
        }
    }
}
