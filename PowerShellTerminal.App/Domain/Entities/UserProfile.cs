using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PowerShellTerminal.App.Domain.Entities
{
    public class UserProfile
    {
        [Key]
        public int ProfileId { get; set; }
        
        public string ProfileName { get; set; } = string.Empty;
        
        public int ThemeId { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public Theme? Theme { get; set; }
        
        public ICollection<CommandHistoryItem> HistoryItems { get; set; } = new List<CommandHistoryItem>();
    }
}