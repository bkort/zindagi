using System.Threading.Tasks;

namespace Zindagi.SeedWork
{
    public interface ILoggedInUser
    {
        Task<VendorId> GetIdentifier();
        Task<long> GetUserId();
        Task<Result<OpenIdUser>> GetUser();
    }
}
