using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    // api/account
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(
            DataContext context,
            ITokenService tokenService,
            IMapper mapper
        )
        {
                _context = context;
                _tokenService = tokenService;
                _mapper = mapper;
        }

        [HttpPost("register")] // api/account/register
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            // ControllerBase automatically binds the body of the request to the
            // values of the DTO -- only make sure that the keys correspond!
            // as in username (json) vs Username (DTO)
            if (await UserExists(registerDTO.Username)) return BadRequest("Username is taken");
            
            var user = _mapper.Map<AppUser>(registerDTO);
            user.UserName = registerDTO.Username.ToLower();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _context.Users
                // the entity framework will not load related entities (Photos)
                // by default - in order to use them, we must eager load them
                // with Include
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => 
                x.UserName == loginDTO.Username
            );
            if (user == null) return Unauthorized("Invalid username");

            return new UserDTO{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
