using BaseLibrary.Response;
using ServerLibrary.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Repositories
{
    public class BaseRepository
    {
        public static GeneralResponse NotFound() => new(false, "Sorry department not found");
        public static GeneralResponse Success() => new(true, "Operation successful");

    }
}
