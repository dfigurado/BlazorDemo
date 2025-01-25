using BaseLibrary.DTOs.Sessions;
using ClientLibrary.Services.Contracts;

namespace ClientLibrary.Helpers
{
    public class CustomHttpHandler(
        HttpClientHelper httpClient, 
        LocalStorageService localStorageService, 
        IUserAccountService accountService) : DelegatingHandler
    {

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var logingUrl = request.RequestUri!.AbsolutePath.Contains("login");
            var registerUrl = request.RequestUri!.AbsolutePath.Contains("register");
            var refreshTokenUrl = request.RequestUri!.AbsolutePath.Contains("refresh");

            if (logingUrl || registerUrl || refreshTokenUrl) return await base.SendAsync(request, cancellationToken);

            var result = await base.SendAsync(request, cancellationToken);

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Get token from localStorage
                // Check if the header contain's token
                var token = await localStorageService.GetToken();
                try
                {
                    token = request.Headers.Authorization!.Parameter;
                }
                catch { }

                var deserializeToken = Serializations.DeserializeJsonString<UserSession>(token);
                if (deserializeToken is null) return result;
                if (string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializeToken.Token);
                    return await base.SendAsync(request, cancellationToken);
                }

                // Call for refresh talem
                var newJwtToken = await GetRefreshToken(deserializeToken.RefreshToken!);
                if (string.IsNullOrEmpty(newJwtToken)) return result;

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newJwtToken);
                return await base.SendAsync(request, cancellationToken);
            }
            return result;
        }

        private async Task<string> GetRefreshToken(string refreshToken)
        {
            var result = await accountService.RefreshTokenAsync(new BaseLibrary.DTOs.RefreshToken() { Token = refreshToken });
            var serializedToken = Serializations.SerializeObj(new UserSession() { Token = result.Token, RefreshToken = result.Token});
            await localStorageService.SetToken(serializedToken);
            return result.Token;
        }
    }
}
