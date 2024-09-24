using AutoMapper;
using HumanShop.Data;
using HumanShop.DTOs;
using HumanShop.Entities;
using HumanShop.Extensions;
using HumanShop.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HumanShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IUserRespository userRespository, IMapper mapper, IPhotoService photoService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUser()
        {
            var user = await userRespository.GetUsersAsync();
            var userToReturn = mapper.Map<IEnumerable<MemberDTO>>(user);
            return Ok(userToReturn);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<MemberDTO>> GetUserById(int id)
        //{
        //    var user = await userRespository.GetUserByIdAsync(id);
        //    if (user == null) { return NotFound(); }
        //    return mapper.Map<MemberDTO>(user);
        //}

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetMemberByUsername(string username)
        {
            var user = await userRespository.GetUserByNameAsync(username);
            if (user == null) { return NotFound(); }
            return mapper.Map<MemberDTO>(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMember(MemberUpdateDTO member)
        {

            var user = await userRespository.GetUserByNameAsync(User.GetUsername());
            if (user == null)
            {
                return BadRequest("Not found username");
            }
            mapper.Map(member, user);

            if (await userRespository.SaveAllAsync()) { return NoContent(); }
            return BadRequest("Failed to save to database");

        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await userRespository.GetUserByNameAsync(User.GetUsername());
            if (user == null) { return BadRequest("Cannot update user"); }
            var result = await photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                URL = result.SecureUri.AbsoluteUri,
                PublicID = result.PublicId
            };
            user.Photos.Add(photo);
            if (await userRespository.SaveAllAsync()) { return CreatedAtAction(nameof(GetUser), new { username = user.UserName }, mapper.Map<PhotoDTO>(photo)); }
            return BadRequest("Something error when upload photo");
        }

        [HttpPut("set-main-photo/{photoID:int}")]
        public async Task<IActionResult> SetMainPhoto([FromRoute] int photoID)
        {
            var user = await userRespository.GetUserByNameAsync(User.GetUsername());
            if (user == null) { return BadRequest("Not found user"); }
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoID);
            if (photo == null) { return BadRequest("Can't set this to main photo"); }

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) { currentMain.IsMain = false; }
            photo.IsMain = true;

            if (await userRespository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Something is error");

        }
        [HttpDelete("delete-photo/{photoId:int}")]
        public async Task<IActionResult> DeletePhoto([FromRoute] int photoId)
        {
            var user = await userRespository.GetUserByNameAsync(User.GetUsername());
            if (user == null) { return BadRequest("Can't find username"); }
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null || photo.IsMain) { return BadRequest("Can't delete main photo!"); }
            if (photo.PublicID != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicID);
                if (result.Error != null)
                {
                    return BadRequest(result.Error);
                }
            }
            user.Photos.Remove(photo);
            if (await userRespository.SaveAllAsync()) { return NoContent(); }
            return BadRequest("Something error when try deleting this photo");
        }

    }
}
