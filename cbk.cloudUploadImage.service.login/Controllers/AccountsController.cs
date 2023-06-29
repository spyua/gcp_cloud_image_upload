using cbk.cloudUploadImage.Infrastructure.Help.Internet;
using cbk.cloudUploadImage.service.login.Model;
using cbk.cloudUploadImage.service.login.Service;
using JWT.Algorithms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cbk.cloudUploadImage.service.login.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ILoginService _loginService;

        // Testing
        private readonly JwtHelpers _jwt;

        public AccountsController(ILoginService loginService, JwtHelpers jwt)
        {
            _loginService = loginService;
            _jwt = jwt;
        }

        
        [Authorize] // 使用此標籤確保此 Action 需要授權
        [HttpGet("username")]
        public IActionResult GetUsername()
        {
            // 在 Controller 中，User 是一個內建的屬性，用來取得當前用戶的 ClaimsPrincipal
            var username = User.Identity?.Name;

            if (username == null)
            {
                return Unauthorized();
            }

            return Ok(username);
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public IActionResult SignIn(Account login)
        {
            if (ValidateUser(login))
            {
                var token = _jwt.GenerateToken(login.Username);
                return Ok(new { token });
            }
            else
            {
                return BadRequest();
            }
        }
        private bool ValidateUser(Account login)
        {
            // 这里你应该添加验证用户的逻辑
            return true;
        }

        [HttpPost]
        public async Task<ActionResult<AccountCreateResponse>> CreateAccount(Account model)
        {
            var account = await _loginService.CreateAccount(model.Username, model.Password);
            
            if (account == null)
            {
                return BadRequest("Account already exists.");
            }

            var token = _loginService.GenerateJwtToken(account);

            return new AccountCreateResponse { Status = "OK", Token = token };
        }

    }
}
