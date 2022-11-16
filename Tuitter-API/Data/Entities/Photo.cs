using System.Security.Cryptography;
using System.Text;

namespace Tuitter_API.Data.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public string PhotoName { get; set; }
        public string PhotoPath { get; set; }
        public bool IsProfilePicture { get; set; }
    }
}
