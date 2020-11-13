using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoMysqlApi.DTOs;
using TodoMysqlApi.Models;
using TodoMysqlApi.Services;

namespace TodoMysqlApi.Controllers
{
  [Route("api/v1/Account/[controller]")]
  [ApiController]
  public class LoginController : ControllerBase {
    private readonly TodoContext _context;
    private readonly IUserService _userService;

    public LoginController(TodoContext context, IUserService userService)
    {
      _context = context;
      _userService = userService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult> getLogin() {
      return await Task.Run(() => NoContent());
    }

    [HttpPost]
    public async Task<ActionResult> doLogin(LoginRequestDTO loginRequestDTO)
    {
      var user = await _userService.Authenticate(loginRequestDTO.UserName, loginRequestDTO.Password);

      if (user == null)
      {
          return Unauthorized();
      }

      var claims = new List<Claim>
      {
          new Claim(ClaimTypes.Name, user.UserName),
          new Claim(ClaimTypes.Role, "Administrator"),
      };
      if (user.Email != null) {
        claims.Add(
          new Claim("Email", user.Email));
      }

      var claimsIdentity = new ClaimsIdentity(
          claims, CookieAuthenticationDefaults.AuthenticationScheme);

      var authProperties = new AuthenticationProperties
      {
          //AllowRefresh = <bool>,
          // Refreshing the authentication session should be allowed.

          //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
          // The time at which the authentication ticket expires. A
          // value set here overrides the ExpireTimeSpan option of
          // CookieAuthenticationOptions set with AddCookie.

          //IsPersistent = true,
          // Whether the authentication session is persisted across
          // multiple requests. When used with cookies, controls
          // whether the cookie's lifetime is absolute (matching the
          // lifetime of the authentication ticket) or session-based.

          //IssuedUtc = <DateTimeOffset>,
          // The time at which the authentication ticket was issued.

          //RedirectUri = <string>
          // The full path or absolute URI to be used as an http
          // redirect response value.
      };

      await HttpContext.SignInAsync(
          CookieAuthenticationDefaults.AuthenticationScheme,
          new ClaimsPrincipal(claimsIdentity),
          authProperties);

      var cookieOptions = new CookieOptions
      {
        Secure = true,
        SameSite = SameSiteMode.Lax
      };

      Response.Cookies.Append("mycookie", "cookievalue", cookieOptions);

      return NoContent();
    }

  }
}