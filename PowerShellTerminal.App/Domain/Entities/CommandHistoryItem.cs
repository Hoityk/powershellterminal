using System;
using System.ComponentModel.DataAnnotations;

namespace PowerShellTerminal.App.Domain.Entities
{
    public class CommandHistoryItem
    {
        [Key]
        public int HistoryId { get; set; }
        
        public int ProfileId { get; set; } 
        
        public string CommandText { get; set; } = string.Empty;
        public DateTime ExecutedAt { get; set; }
        public bool IsSuccess { get; set; }
        public UserProfile? Profile { get; set; }
    }
}