using Blazored.LocalStorage;

namespace ClientLibrary.Helpers
{
    public class LocalStorageService(ILocalStorageService localStorageService)
    {
        private const string _storageKey = "authentication-token";

        public async Task<string> GetToken() => await localStorageService.GetItemAsStringAsync(_storageKey);
        public async Task SetToken(string token) => await localStorageService.SetItemAsStringAsync(_storageKey, token); 
        public async Task RemoveToken() => await localStorageService.RemoveItemAsync(_storageKey);
    }
}
