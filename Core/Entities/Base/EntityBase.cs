using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Base
{
    public class EntityBase : IIdentifiable
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }
}
