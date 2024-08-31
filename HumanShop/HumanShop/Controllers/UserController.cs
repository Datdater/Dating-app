using AutoMapper;
using HumanShop.Data;
using HumanShop.DTOs;
using HumanShop.Entities;
using HumanShop.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HumanShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserRespository userRespository, IMapper mapper) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUser()
        {
            var user = await userRespository.GetUsersAsync();
            var userToReturn = mapper.Map<IEnumerable<MemberDTO>>(user);
            return Ok(userToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDTO>> GetUserById(int id)
        {
            var user = await userRespository.GetUserByIdAsync(id);
            if (user == null) { return NotFound(); }
            return mapper.Map<MemberDTO>(user);
        }
    }
}
