using System.Threading.Tasks;
using ProjectsAgenda.Common.Models;
namespace ProjectsAgenda.Common.Services
{
    public interface IApiService
    {
        Task<Response<PartnerResponse>> GetPartnerByEmailAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            string email);
        Task<Response<TokenResponse>> GetTokenAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            TokenRequest request);

        Task<bool> CheckConnectionAsync(string url);
    }
}
