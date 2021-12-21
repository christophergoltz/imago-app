using System.Threading.Tasks;

namespace ImagoApp
{
    public interface ILocalFileService
    {
        void OpenFolder(string path);
        Task<bool> SaveFileWithDialog(string sourceFile);
        Task<string> OpenAndCopyFileToFolder(string folder);
    }
}