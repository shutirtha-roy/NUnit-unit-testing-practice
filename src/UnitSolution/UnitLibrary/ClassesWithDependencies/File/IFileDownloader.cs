namespace UnitLibrary.ClassesWithDependencies.File
{
    public interface IFileDownloader
    {
        void DownloadFile(string url, string path);
    }
}