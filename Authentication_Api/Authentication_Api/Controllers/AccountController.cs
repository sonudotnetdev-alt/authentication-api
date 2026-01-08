using Authentication.Core.Dto;
using Authentication.Core.Entities;
using Authentication.Core.Interface;
using Authentication_Api.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _usersManager;
        private readonly SignInManager<AppUser> _signInManager;
        //private readonly IMapper _mapper;
        private readonly ITokenService _tokenServices;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenServices, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _usersManager = userManager;
            //_mapper = mapper;
            _tokenServices = tokenServices;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _usersManager.FindByEmailAsync(dto.Email);
            if (user == null) return Unauthorized();
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (result.Succeeded == false || result is null) return Unauthorized();
            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email ?? string.Empty, // Fix for CS8601
                Token = _tokenServices.CreateToken(user)
            });
        }


        [HttpPost("Register")]
        public async Task<ActionResult> post(RegistrationDto Dto)
        {
            if (CheckEmailExist(Dto.Email).Result.Value)
            {
                return BadRequest("This email has already Token");
            }

            var user = new AppUser
            {
                DisplayName = Dto.DisplayName,
                UserName = Dto.Email,
                Email = Dto.Email,
            };            
            var result = await _usersManager.CreateAsync(user, Dto.Password);
            if (result.Succeeded == false || result is null) return BadRequest("400");
            return Ok(new UserDto
            {
                DisplayName = Dto.DisplayName,
                Email = Dto.Email,
                Token = _tokenServices.CreateToken(user),
            });
        }

        [HttpGet("check-email-exist")]
        public async Task<ActionResult<bool>> CheckEmailExist([FromQuery] string email)
        {
            var result = await _usersManager.FindByEmailAsync(email);
            if (result is not null)
            {
                return true;
            }
            return false;
        }

        [Authorize]
        [HttpGet("test")]
        public ActionResult <string> test()
        {
            return "hi";
        } 

        [Authorize]
        [HttpGet("get-current-user")]
        public async Task<IActionResult> getCurrentUser()
        {
            var user = await _usersManager.FindEmailByClaimsPrinciple(HttpContext.User);
            if (user == null) return BadRequest("User Not Found");
            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email ?? string.Empty, // Fix for CS8601
                Token = _tokenServices.CreateToken(user)
            });
        }

        [Authorize]
        [HttpGet("get-user-address")]
        public async Task<IActionResult> getUserAddress()
        {
            var user = _usersManager.FindUserByClaimsPrincipleWithAddress(HttpContext.User);
            if (user == null) return BadRequest("Not Found");
            return Ok(user);
        }

        [Authorize]
        [HttpPut("update-user-address")]
        public async Task<IActionResult> UpdateUserAddress(AddressDto dto)
        {
            var user = await _usersManager.FindUserByClaimsPrincipleWithAddress(HttpContext.User);

            user.address = new Address
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                City = dto.City,
                State = dto.State,
                Street = dto.Street,
                PostalCode = dto.PostalCode,
            };
            var result = await _usersManager.UpdateAsync(user);
            if (result.Succeeded) return Ok(user.address);
            return BadRequest($"Problem in updating this {HttpContext.User}");
        }
    }
}
