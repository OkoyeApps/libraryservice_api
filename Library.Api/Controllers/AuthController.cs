using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Domain.Dtos;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            this._authService = authService;
            this._mapper = mapper;
        }
        public IActionResult Index()
        {
            return Ok();
        }

        public async Task<IActionResult> Register(UserRegistrationDto model)
        {
            var userModel = _mapper.Map<UserRegistrationDto, User>(model);
          var result = await  _authService.RegisterUser(userModel);
            if (!result)
            {
              return   BadRequest();
            }
            return Ok(new { message = "Registration successful" });
        }
    }
}
