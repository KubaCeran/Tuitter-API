/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Tuitter_API.Controllers;

public class PhotosController : BaseApiController
{
    private readonly IPhotoRepository _photoRepository;
    private readonly ILoggedUserService _loggedUserService;

    public PhotosController(IPhotoRepository photoRepository, ILoggedUserService loggedUserService)
    {
        _photoRepository = photoRepository;
        _loggedUserService = loggedUserService;
    }

    [HttpPost("add")]
    public async Task<ActionResult> AddPhoto(bool isProfilePicture, [FromForm] IFormFile photo)
    {
            if(photo == null || photo.FileName == null || photo.FileName.Length == 0)
            {
                return BadRequest("File not selected");
            }

        var userId = await _loggedUserService.GetLoggedUserId(User);

        if(!await _photoRepository.AddImage(userId, isProfilePicture, photo))
        {
            return BadRequest("Can't add photo");
        }
        return Ok();

    }

    [HttpDelete("delete")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var userId = await _loggedUserService.GetLoggedUserId(User);

        var response = await _photoRepository.DeleteImage(userId, photoId);
        if(response.IsError)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Message);
    }


    [HttpGet("{photoId}")]
    [AllowAnonymous]
    public async Task<ActionResult> GetPhotoById(int photoId)
    {
        var photo = await _photoRepository.GetImageById(photoId);

        if(photo != null)
        {
            var fileBytes = System.IO.File.ReadAllBytes(photo.PhotoPath);
            new FileExtensionContentTypeProvider().TryGetContentType(Path.GetFileName(photo.PhotoPath), out var contentType);
            return new FileContentResult(fileBytes, contentType ?? "image");
        };

        return NotFound("Photo not found");
    }
    [HttpGet("user/{userId}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<PhotoDto>>> GetListOfPhotosForUser(int userId)
    {
        var photoList = await _photoRepository.GetListOfPhotoInfoForUser(userId);

        if (photoList.Count > 0)
        {
            return Ok(photoList);
        }
        return NotFound("No photos for user");
    }

    [HttpGet("info/{photoId}")]
    [AllowAnonymous]
    public async Task<ActionResult<PhotoDto>> GetPhotoInfoById(int photoId)
    {
        var photo = await _photoRepository.GetImageById(photoId);
        if (photo != null)
        {
            return new PhotoDto
            {
                UserId = photo.UserId,
                PhotoId = photo.Id,
                FileName = photo.PhotoName,
                IsProfilePicture = photo.IsProfilePicture,
            };
        };

        return NotFound("Photo not found");
    }

    [HttpPut("set-profile-picture")]
    public async Task<ActionResult> SetProfilePicture(int photoId)
    {
        var userId = await _loggedUserService.GetLoggedUserId(User);

        var response = await _photoRepository.SetProfilePicture(userId,photoId);

        if (response.IsError)
            return BadRequest(response.Message);
        return Ok(response.Message);
    }
}

*/