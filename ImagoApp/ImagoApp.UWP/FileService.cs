using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;

namespace ImagoApp.UWP
{
    public class FileService : IFileService
    {
        public async void OpenFolder(string path)
        {
            try
            {
                var folder = await StorageFolder.GetFolderFromPathAsync(path);
                await Launcher.LaunchFolderAsync(folder);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
