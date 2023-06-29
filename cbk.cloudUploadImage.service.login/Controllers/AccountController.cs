using cbk.cloudUploadImage.Infrastructure.Help.Internet;
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



        [HttpGet]
        public async Task<ApiResponse<AccountDto>> Query([FromQuery] AccountCreate query)
        {
            //PaginationList<AccountDto> dto = await _accountSearch.QueryAsync(query);
            return new ApiResponse<AccountDto>
            {
                Message = "Query Success",
                Data = new AccountDto() { Username="123",Password="123"}
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
                var token = _jwt.GenerateToken2(login.UserName);
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


      
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AccountDto>>> CreateAccount(AccountCreate model)
        {
            var account = await _loginService.CreateAccount(model.UserName, model.UserName);
            account = null;
            if (account == null)
            {
                return BadRequest("Account already exists.");
            }

            var token = _loginService.GenerateJwtToken(account);

            return Ok(new ApiResponse<AccountDto>
            {
                Message = "Create Success",
                Data = new AccountDto() { Username = model.UserName, Password = model.Password }
            });
        }

    }
}
