using api.DTOs;
using api.DTOs.Account;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;

namespace api.Controllers;

    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : GenericControllerBase<ICrudService, GetAccountDto, CreateAccountDto, UpdateAccountDto>
    {
        private readonly RequestHandler _requestHandler;

        public AccountController(ICrudService service, RequestHandler requestHandler) 
            : base(service, "account")
        {
            _requestHandler = requestHandler;
        }

        [HttpPut]
        [Route("getByName")]
        public ResponseDto GetAccountByName([FromBody] GetAccountByNameDto username)
        {
            return ValidateAndProceed(
                () => Service.GetSingleItemByParameters<GetAccountDto>(TableName, 
                    new Dictionary<string, object> { { "name", username.Name } }), 
                $"fetched account with name {username.Name}");
        }

        [HttpPut]
        [Route("getByEmail")]
        public ResponseDto GetAccountByEmail([FromBody] GetAccountByEmailDto email)
        {
            return ValidateAndProceed(
                () => Service.GetSingleItemByParameters<GetAccountDto>(TableName, 
                    new Dictionary<string, object> { { "email", email.Email } }), 
                $"fetched account with email {email.Email}");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var token = Service.VerifyLogin(dto.Username, dto.Password);
            if (token != null)
            {
                return Ok(new ResponseDto { ResponseData = token });
            }
            return Unauthorized(new ResponseDto { MessageToClient = "Wrong username or password" });
        }

        
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public ResponseDto Register([FromBody] CreateAccountDto dto)
        {
            return ValidateAndProceed(() => Service.CreateAccount(new Dictionary<string, object>
                {
                    {"name", dto.Name},
                    {"password", dto.Password},
                    {"email", dto.Email}
                }), 
                "executed registry action");
        }
    }
