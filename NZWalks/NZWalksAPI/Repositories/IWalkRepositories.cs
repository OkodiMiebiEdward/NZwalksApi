using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Repositories
{
    public interface IWalkRepositories
    {
        Task<Walk> CreateAsync(Walk walk);
    }
}
