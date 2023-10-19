using System.ComponentModel.DataAnnotations;

namespace Tower_Section_Catalogue.Models
{
    public class TowerSection
    {
        [Key]

        public int Id { get; set; }

        public string partNumber { get; set; }

        public double bottomDiameter { get; set; }

        public double topDiameter { get; set; }

        public double lenght { get; set;}

        public ICollection<Shell>? Shells { get; set; }
    }
}
