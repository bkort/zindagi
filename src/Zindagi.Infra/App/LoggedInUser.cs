using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Zindagi.Domain.UserAggregate;
using Zindagi.SeedWork;

namespace Zindagi.Infra.App
{
    public class LoggedInUser : ILoggedInUser
    {
        private readonly AuthenticationStateProvider _authProvider;
        private readonly IUserRepository _userRepository;

        public LoggedInUser(AuthenticationStateProvider authProvider, IUserRepository userRepository)
        {
            _authProvider = authProvider;
            _userRepository = userRepository;
        }

        public async Task<VendorId> GetIdentifier()
        {
            var user = (await _authProvider.GetAuthenticationStateAsync()).User;
            return user?.Identity?.IsAuthenticated ?? false ? user.GetIdentifier() : null;
        }

        public async Task<long> GetUserId()
        {
            var identifier = (await _authProvider.GetAuthenticationStateAsync()).User?.GetIdentifier();

            if (identifier == null)
            {
                return default;
            }

            var user = await _userRepository.GetAsync(identifier.Value);

            return user.Id;
        }

        public async Task<Result<OpenIdUser>> GetUser()
        {
            var authState = await _authProvider.GetAuthenticationStateAsync();

            if (authState.User.Identity?.IsAuthenticated ?? false)
                return Result<OpenIdUser>.Success(OpenIdUser.Create(authState.User));

            return Result<OpenIdUser>.Error("User not logged in");
        }
    }
}
