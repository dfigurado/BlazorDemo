using BaseLibrary.DTOs;
using BaseLibrary.DTOs.Sessions;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Pipes;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Helpers
{
    public class CustomAuthenticationStateProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var stringToken = await localStorageService.GetToken();
            if (string.IsNullOrEmpty(stringToken))
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }

            var deserializeToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
            if (deserializeToken == null) return await Task.FromResult(new AuthenticationState(_anonymous));

            var getUserClaims = DecryptToken(deserializeToken.Token);
            if (getUserClaims == null) return await Task.FromResult(new AuthenticationState(_anonymous));

            var claimPrincipal = SetClaimPrincipal(getUserClaims);
            return await Task.FromResult(new AuthenticationState(claimPrincipal));
        }

        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if (userSession.Token != null || userSession.RefreshToken != null)
            {
                var serializeSession = Serializations.SerializeObj(userSession);
                await localStorageService.SetToken(serializeSession);
                var getUserClaims = DecryptToken(userSession.Token);
                claimsPrincipal = SetClaimPrincipal(getUserClaims);
            }
            else
            {
                await localStorageService.RemoveToken();
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims userClaims)
        {
            if (userClaims.Email is null) return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userClaims.Id!),
                new Claim(ClaimTypes.Name, userClaims.Name!),
                new Claim(ClaimTypes.Email, userClaims.Email!),
                new Claim(ClaimTypes.Role, userClaims.Role!)
            }, "JwtAuth"));
        }

        private static CustomUserClaims DecryptToken(string? jwt)
        {
            if (string.IsNullOrEmpty(jwt)) return new CustomUserClaims();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            var userId = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier);
            var userName = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name);
            var userEmail = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Email);
            var userRole = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Role);
            return new CustomUserClaims(userId?.Value ?? "", userName?.Value ?? "", userEmail?.Value ?? "", userRole?.Value ?? "");
        }
    }
}
