using cbk.cloudUploadImage.Infrastructure.Security;
using cbk.cloudUploadImage.service.login.Dto;
using cbk.cloudUploadImage.service.login.Model;
using cbk.cloudUploadImage.service.login.Service;
using JWT.Algorithms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cbk.cloudUploadImage.service.login.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ILoginService _loginService;

        // Testing
        private readonly JwtHelpers _jwt;

        public AccountController(ILogger<AccountController> logger
                               , ILoginService loginService
                               , JwtHelpers jwt)
        {
            _logger = logger;
            _loginService = loginService;
            _jwt = jwt;
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AccountDto>>> CreateAccount(AccountCreate model)
        {
            // Invalid (待寫..或後續轉到Middleware)

            var account = await _loginService.CreateAccount(model.UserName, model.Password);

            if (account == null)
                return BadRequest("Account already exists.");

            return Ok(new ApiResponse<AccountDto>
            {
                Message = "Create Account Success",
                Data = new AccountDto() { Token = account.Token  
                                        ,UserName = account.UserName
                                        , Password = account.Password }
            });
        }




        [HttpGet]
        public async Task<ApiResponse<AccountDto>> Query([FromQuery] AccountCreate query)
        {
            //PaginationList<AccountDto> dto = await _accountSearch.QueryAsync(query);
            return new ApiResponse<AccountDto>
            {
                Message = "Query Success",
                Data = new AccountDto() { UserName="123",Password="123"}
            };
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
        public IActionResult SignIn(AccountCreate login)
        {
            if (ValidateUser(login))
            {
                var token = _jwt.GenerateToken(login.UserName);
                return Ok(new { token });
            }
            else
            {
                return BadRequest();
            }
        }
        private bool ValidateUser(AccountCreate login)
        {
            // 这里你应该添加验证用户的逻辑
            return true;
        }


      
        
    }
}
