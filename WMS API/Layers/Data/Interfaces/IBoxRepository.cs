using WMS_API.Models.Boxes;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface IBoxRepository
    {
        Task<List<BoxData>> GetAllBoxesAsync();
        Task<BoxData> GetBoxByIdAsync(Guid boxId);
        Task<List<BoxData>> GetBoxHistoryAsync(Guid boxId);
        Task AddBoxAsync(Box box);
        Task AddBoxDataAsync(BoxData boxData);
    }
}
