using BaseLibrary.Entities;
using BaseLibrary.Response;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Persistence.Context;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementation
{
    public class CityRepository(AppDbContext appDbContext) : BaseRepository, IGenericRepositoryInterface<City>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var result = await appDbContext.Cities.FindAsync(id);
            if (result is null) return NotFound();
            appDbContext.Cities.Remove(result);
            await Commit();
            return Success();
        }
        public async Task<List<City>> GetAll()
        {
            var result = await appDbContext.Cities.ToListAsync();
            return result;
        }
        public async Task<City> GetById(int id)
        {
            var result = await appDbContext.Cities.FindAsync(id);
            return result!;
        }
        public async Task<GeneralResponse> Insert(City item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false, "City already exists");
            appDbContext.Cities.Add(item);
            await Commit();
            return Success();
        }
        public async Task<GeneralResponse> Update(City item)
        {
            var result = await appDbContext.Cities.FindAsync(item.Id);
            if (result is null) return NotFound();
            result.Name = item.Name;
            await Commit();
            return Success();
        }
        private async Task Commit() => await appDbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var item = await appDbContext.Cities.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
            return item is null;
        }
    }
}
