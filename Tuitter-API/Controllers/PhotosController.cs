using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tuitter_API.Repository.Photo;
using Tuitter_API.Service.User;

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

    [HttpGet("user/{userId}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<PhotoDto>>> GetPhotosForUser(int userId)
    {

        var listPhotoDto = await _photoRepository.GetAllImagesForUser(userId);

        if (listPhotoDto == null || listPhotoDto.Count() == 0)
            return BadRequest("No photos found");
        return Ok(listPhotoDto);
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

