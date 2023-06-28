using cbk.cloudUploadImage.service.login.Model;
using cbk.cloudUploadImage.service.login.Service;
using Microsoft.AspNetCore.Mvc;

namespace cbk.cloudUploadImage.service.login.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AccountsController(ILoginService loginService)
        {
            _loginService = loginService;
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
