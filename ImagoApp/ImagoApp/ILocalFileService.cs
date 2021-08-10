using System.Threading.Tasks;

namespace ImagoApp
{
    public interface ILocalFileService
    {
        void OpenFolder(string path);
        Task SaveFile(string sourceFile);
    }
}