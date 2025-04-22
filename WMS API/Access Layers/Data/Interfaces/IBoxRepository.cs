using WMS_API.Models.Boxes;

namespace WMS_API.Layers.Data.Interfaces
{
    public interface IBoxRepository
    {
        Task<List<Box>> GetAllBoxesAsync();
        Task<Box> GetBoxByIdAsync(Guid boxId);
        Task<BoxData> GetBoxDataByIdAsync(Guid boxId);
        Task AddBoxAsync(Box box);
        Task AddBoxDataAsync(BoxData boxData);
    }
}
