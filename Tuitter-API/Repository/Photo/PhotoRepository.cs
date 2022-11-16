using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuitter_API.Data.DataContext;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Tuitter_API.Repository.Photo
{
    public interface IPhotoRepository
    {
        Task<bool> AddImage(int userId, bool isProfilePicture, IFormFile phot);
        Task<SetPhotoResponse> DeleteImage(int userId, int photoId);
        Task<List<PhotoDto>> GetAllImagesForUser(int userId);
        Task<SetPhotoResponse> SetProfilePicture(int userId, int photoId);
    }
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _dataContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PhotoRepository(DataContext dataContext, IHostingEnvironment hostingEnvironment)
        {
            _dataContext = dataContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<bool> AddImage(int userId, bool isProfilePicture, IFormFile photo)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "Photos/", photo.FileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
                stream.Close();
            }

            _dataContext.Photos.Add(new Data.Entities.Photo
            {
                UserId = userId,
                PhotoName = photo.FileName,
                PhotoPath = path,
                IsProfilePicture = isProfilePicture,
            });

            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<SetPhotoResponse> DeleteImage(int userId, int photoId)
        {
            var photo = await _dataContext.Photos.Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.Id == photoId);

            if(photo == null)
                return new SetPhotoResponse { IsError = true, Message = "Photo wasn't found" };

            File.Delete(photo.PhotoPath);
            _dataContext.Photos.Remove(photo);

            await _dataContext.SaveChangesAsync();

            return new SetPhotoResponse { IsError = false, Message = "Photo deleted successfuly" };
        }

        public async Task<List<PhotoDto>> GetAllImagesForUser(int userId)
        {
            var photos = await _dataContext.Photos.Where(x => x.UserId == userId).ToListAsync();

            var listPhotoDto = new List<PhotoDto>();
            foreach(var photo in photos)
            {
                Byte[] b = File.ReadAllBytes(photo.PhotoPath);
                var file = new FileContentResult(b, "Image/" + photo.PhotoName.Split('.').Last());
                listPhotoDto.Add(new PhotoDto
                {
                    UserId = photo.UserId,
                    PhotoId = photo.Id,
                    FileName = photo.PhotoName,
                    ContentType = file.ContentType,
                    IsProfilePicture = photo.IsProfilePicture,
                    FileContent = file.FileContents
                });
            }
            return listPhotoDto;
        }

        public async Task<SetPhotoResponse> SetProfilePicture(int userId, int photoId)
        {
            var photo = await _dataContext.Photos.Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.Id == photoId);
            if (photo == null)
                return new SetPhotoResponse { IsError = true, Message = "Photo wasn't found" };
            if (photo.IsProfilePicture)
                return new SetPhotoResponse { IsError = true, Message = "It is already a  profile picture" };
            
            var previousProfilePicture = await _dataContext.Photos.Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.IsProfilePicture == true);
            
            if(previousProfilePicture != null)
            {
                previousProfilePicture.IsProfilePicture = false;
            }
            photo.IsProfilePicture = true;

            await _dataContext.SaveChangesAsync();

            return new SetPhotoResponse { IsError = false, Message = "Photo set successfuly" };
        }
    }
}
