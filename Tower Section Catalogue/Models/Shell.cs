using System.ComponentModel.DataAnnotations;

namespace Tower_Section_Catalogue.Models
{
    public class Shell
    {
        [Key]

        public int ShellPosition { get; set; }
        public double Height { get; set; }
        public double BottomDiameter { get; set; }
        public double TopDiameter { get; set; }
        public double Thickness { get; set; }
        public double SteelDensity { get; set; }

        public int TowerSectionId { get; set; }
        public TowerSection? TowerSection { get; set; }
    }
}
