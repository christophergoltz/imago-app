using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Microsoft.AppCenter.Crashes;

namespace ImagoApp.UWP
{
    public class LocalFileService : ILocalFileService
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
                Debug.WriteLine(exception);
                Crashes.TrackError(exception, new Dictionary<string, string>()
                {
                    { "path", path}
                });
            }
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-save-a-file-with-a-picker
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        public async Task SaveFile(string sourceFile)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop
            };

            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Imago-Datenbank", new List<string>() { ".imagodb" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = Path.GetFileName(sourceFile);

            var selectedTarget = await savePicker.PickSaveFileAsync();
            if (selectedTarget == null)
                return;

            // Prevent updates to the remote version of the file until
            // we finish making changes and call CompleteUpdatesAsync.
            CachedFileManager.DeferUpdates(selectedTarget);
            // write to file
            await FileIO.WriteBytesAsync(selectedTarget, await File.ReadAllBytesAsync(sourceFile));
            // Let Windows know that we're finished changing the file so
            // the other app can update the remote version of the file.
            // Completing updates may require Windows to ask for user input.
            await CachedFileManager.CompleteUpdatesAsync(selectedTarget);
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-using-file-and-folder-pickers
        /// </summary>
        public async Task<string> OpenAndCopyFileToFolder(string folder)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.List,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop
            };

            picker.FileTypeFilter.Add(".imagodb");


            var selectedFile = await picker.PickSingleFileAsync();
            if (selectedFile == null)
                return null;

            var temp = await StorageFolder.GetFolderFromPathAsync(folder);

            var copiedFile = await selectedFile.CopyAsync(temp);

            return copiedFile.Path;
        }
    }
}
