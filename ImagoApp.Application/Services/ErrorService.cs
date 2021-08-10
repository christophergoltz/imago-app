using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AppCenter.Crashes;

namespace ImagoApp.Application.Services
{
    public interface IErrorService
    {
        void TrackException(Exception exception, Dictionary<string, string> properties, string description, string stacktrace, params string[] attachedCharacterDatabaseFiles);
    }

    public class ErrorService : IErrorService
    {
        public void TrackException(Exception exception, Dictionary<string, string> properties,string description, string stacktrace, params string[] attachedCharacterDatabaseFiles)
        {
            var attachments = CreateErrorAttachments(description, stacktrace, attachedCharacterDatabaseFiles);
            if (attachments.Any())
            {
                Crashes.TrackError(exception, properties, attachments);
            }
            else
            {
                Crashes.TrackError(exception, properties);
            }
        }

        private ErrorAttachmentLog[] CreateErrorAttachments(string description, string stacktrace, params string[] attachedCharacterDatabaseFiles)
        {
            var attachments = new List<ErrorAttachmentLog>();

            if (attachedCharacterDatabaseFiles.Any())
            {
                foreach (var characterDatabaseFile in attachedCharacterDatabaseFiles)
                {
                    var fileName = Path.GetFileName(characterDatabaseFile);
                    var databaseFileBytes = File.ReadAllBytes(characterDatabaseFile);
                    attachments.Add(ErrorAttachmentLog.AttachmentWithBinary(databaseFileBytes, fileName,
                        "application/octet-stream"));
                }
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                attachments.Add(ErrorAttachmentLog.AttachmentWithText(description, "description.txt"));
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(stacktrace))
                {
                    attachments.Add(ErrorAttachmentLog.AttachmentWithText(stacktrace, "stacktrace.txt"));
                }
            }
            catch (Exception)
            {
               //ignored
            }

            return attachments.ToArray();
        }
    }
}