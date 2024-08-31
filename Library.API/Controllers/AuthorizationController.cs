﻿using Library.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = Library.Infrastructure.Services.IAuthorizationService;

namespace Library.API.Controllers;


/// <summary>
/// 
/// </summary>
/// <param name="authorizationService"></param>
[ApiController]
[Route("[controller]")]
public class AuthorizationController(IAuthorizationService authorizationService) : ControllerBase
{
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await authorizationService.GenerateTokenAsync(request.Login, request.Password);
        return Ok(token);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        await authorizationService.Register(request);
        return Ok("Registration successful");
    }
    
}