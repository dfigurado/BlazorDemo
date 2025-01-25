using BaseLibrary.Entities;
using BaseLibrary.Response;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Persistence.Context;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementation
{
    public class CountryRepository(AppDbContext appDbContext) : BaseRepository, IGenericRepositoryInterface<Country>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var result = await appDbContext.Countries.FindAsync(id);
            if (result is null) return NotFound();

            appDbContext.Countries.Remove(result);
            await Commit();
            return Success();
        }

        public async Task<List<Country>> GetAll()
        {
            var result = await appDbContext.Countries.ToListAsync();
            return result;
        }

        public async Task<Country> GetById(int id)
        {
            var result = await appDbContext.Countries.FindAsync(id);
            return result!;
        }

        public async Task<GeneralResponse> Insert(Country item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false, "Country already exists");
            appDbContext.Countries.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Country item)
        {
            var result = await appDbContext.Countries.FindAsync(item.Id);
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
