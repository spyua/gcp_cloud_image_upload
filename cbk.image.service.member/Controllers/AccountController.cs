using cbk.image.service.member.Dto;
using cbk.image.service.member.Model;
using cbk.image.service.member.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cbk.image.service.member.Controllers
{
    // 待補版本設計....

    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ILoginService _loginService;

        public AccountController(ILogger<AccountController> logger, ILoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }


        [HttpPost(nameof(Create))]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AccountDto>>> Create(AccountCreate model)
        {
            // Invalid (待寫..或後續轉到Middleware)
            var account = await _loginService.CreateAccount(model.UserName, model.Password);
            return Ok(new ApiResponse<AccountDto>
            {
                Message = "Create Account Success",
                Data = new AccountDto()
                {
                    Token = ""
                    ,
                    UserName = account.UserName
                    ,
                    Password = account.Password
                }
            });
        }

        
        [HttpPost(nameof(Login))]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AccountDto>>> Login(AccountLogin model)
        {
            // Invalid (待寫..或後續轉到Middleware)
            var account = await _loginService.LoginAccount(model.UserName, model.Password);
            return Ok(new ApiResponse<AccountDto>
            {
                Message = "Login Account Success",
                Data = new AccountDto()
                {
                    Token = account.Token
                   ,
                    UserName = account.UserName
                   ,
                    Password = account.Password
                }
            });

              
        }
        /*測試用保留
         
         [Authorize] // 使用此標籤確保此 Action 需要授權
        [HttpGet("username")]
        public IActionResult GetUsername()
        {
            // 在 Controller 中，User 是一個內建的屬性，用來取得當前用戶的 ClaimsPrincipal
            var username = User.Claims.ToList();

            if (username[0].Equals(""))
            {
                return Unauthorized();
            }
            var d = username[0].Value;
            return Ok(d);
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public IActionResult SignIn(AccountCreate login)
        {
            var token = _jwtService.GenerateToken(login.UserName);
            return Ok(new { token });
        }        
         */
    }
}
