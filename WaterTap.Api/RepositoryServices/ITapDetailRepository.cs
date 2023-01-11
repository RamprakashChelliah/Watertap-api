using WaterTap.Api.Models;

namespace WaterTap.Api.RepositoryServices
{
    public interface ITapDetailRepository
    {
        List<TapDetailModel> GetAllTapDetails();
        TapDetailModel? GetTapDetail(int id);
        int AddNewTapDetail(TapDetailModel tapInfo);
        Task<int> UpdateTapDetail(int id, TapDetailModel tapInfo);
        Task<int> DeleteTapDetail(int id);
    }
}
