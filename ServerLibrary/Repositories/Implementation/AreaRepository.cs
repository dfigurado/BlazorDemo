using BaseLibrary.Entities;
using BaseLibrary.Response;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Persistence.Context;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementation
{
    public class AreaRepository(AppDbContext appDbContext) : BaseRepository, IGenericRepositoryInterface<Area>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var result = await appDbContext.Areas.FindAsync(id);
            if (result is null) return NotFound();

            appDbContext.Areas.Remove(result);
            await Commit();
            return Success();
        }

        public async Task<List<Area>> GetAll()
        {
            var result = await appDbContext.Areas.ToListAsync();
            return result;
        }

        public async Task<Area> GetById(int id)
        {
            var result = await appDbContext.Areas.FindAsync(id);
            return result!;
        }

        public async Task<GeneralResponse> Insert(Area item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false, "Area already exists");
            await appDbContext.Areas.AddAsync(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Area item)
        {
            var result = await appDbContext.Areas.FindAsync(item.Id);
            if (result is null) return NotFound();
            result.Name = item.Name;
            await Commit();
            return Success();
        }

        private async Task Commit() => await appDbContext.SaveChangesAsync();

        private async Task<bool> CheckName(string name)
        {
            var item = await appDbContext.Areas.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
            return item is null;
        }
    }
}
