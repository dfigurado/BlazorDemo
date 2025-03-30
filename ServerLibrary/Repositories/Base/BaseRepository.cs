using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Base
{
    public class BaseRepository
    {
        protected static GeneralResponse NotFound() => new(false, "Sorry department not found");
        protected static GeneralResponse Success() => new(true, "Operation successful");

    }
}