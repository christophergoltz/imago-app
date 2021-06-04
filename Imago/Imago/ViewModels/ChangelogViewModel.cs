using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Repository;

namespace Imago.ViewModels
{
    public class ChangelogViewModel
    {
        public List<ChangeLogEntry> Changelog { get; set; }

        public ChangelogViewModel(IChangeLogRepository changeLogRepository)
        {
            Changelog = changeLogRepository.GetChangeLogEntries();
        }
    }
}
