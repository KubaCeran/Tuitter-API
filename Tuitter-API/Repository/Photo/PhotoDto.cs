namespace Tuitter_API.Repository.Photo
{
    public class PhotoDto
    {
        public int UserId { get; set; }
        public int PhotoId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public bool IsProfilePicture { get; set; }
        public Byte[] FileContent { get; set; }
    }
}
