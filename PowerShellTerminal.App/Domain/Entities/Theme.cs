using System.ComponentModel.DataAnnotations;

namespace PowerShellTerminal.App.Domain.Entities
{
    public class Theme
    {
        [Key]
        public int ThemeId { get; set; }
        public string ThemeName { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = "#000000"; 
        public string ForegroundColor { get; set; } = "#FFFFFF"; 
        public string CursorColor { get; set; } = "#FFFFFF";
    }
}