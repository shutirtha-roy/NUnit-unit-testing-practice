using FileIO = System.IO.File;

namespace UnitLibrary.ClassesWithDependencies.Reader
{
    public class FileReader : IFileReader
    {
        public string Read(string path)
        {
            return FileIO.ReadAllText(path);
        }
    }
}
