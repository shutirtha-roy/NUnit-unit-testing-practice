
namespace UnitLibrary.ClassesWithDependencies.Repository
{
    public interface IVideoRepository
    {
        IEnumerable<Video> GetUnProcessedVideos();
    }
}