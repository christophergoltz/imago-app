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
        void TrackException(Exception exception, Dictionary<string, string> properties,
            bool includeCharacterDatabase, string description, string stacktrace);
    }

    public class ErrorService : IErrorService
    {
        private readonly string _characterDatabaseFile;

        public ErrorService(string characterDatabaseFile)
        {
            _characterDatabaseFile = characterDatabaseFile;
        }

        public void TrackException(Exception exception, Dictionary<string, string> properties,
            bool includeCharacterDatabase, string description, string stacktrace)
        {
            var attachments = CreateErrorAttachments(includeCharacterDatabase, description, stacktrace);
            if (attachments.Any())
            {
                Crashes.TrackError(exception, properties, attachments);
            }
            else
            {
                Crashes.TrackError(exception, properties);
            }
        }

        private ErrorAttachmentLog[] CreateErrorAttachments(bool includeCharacterDatabase, string description, string stacktrace)
        {
            var attachments = new List<ErrorAttachmentLog>();

            if (includeCharacterDatabase)
            {
                var databaseFileBytes = File.ReadAllBytes(_characterDatabaseFile);
                attachments.Add(ErrorAttachmentLog.AttachmentWithBinary(databaseFileBytes, "ImagoApp_Character.db",
                    "application/octet-stream"));
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