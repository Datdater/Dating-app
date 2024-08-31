using System.ComponentModel.DataAnnotations.Schema;

namespace HumanShop.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public required string URL { get; set; }
        public bool IsMain { get; set; }
        public string? PublicID { get; set; }

        public int AppUserID { get; set; }
        public AppUser AppUser { get; set; } = null!;
    }
}