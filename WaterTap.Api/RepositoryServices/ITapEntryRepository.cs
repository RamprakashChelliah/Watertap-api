using WaterTap.Api.Models;

namespace WaterTap.Api.RepositoryServices
{
    public interface ITapEntryRepository
    {
        List<TapEntryDetailModel> GetAllTapEntryDetails();
        TapEntryDetailModel? GetTapEntryDetail(int id);
        int AddNewTapEntryDetail(TapEntryDetailModel tapInfo);
        Task<int> UpdateTapEntryDetail(int id, TapEntryDetailModel tapInfo);
        Task<int> DeleteTapEntryDetail(int id);
    }
}
