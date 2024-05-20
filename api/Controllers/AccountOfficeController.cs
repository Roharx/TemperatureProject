using api.DTOs;
using api.DTOs.AccountOffice;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;

namespace api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class AccountOfficeController : GenericControllerBase<ICrudService, GetAccountOfficeDto, CreateAccountOfficeDto, UpdateAccountOfficeDto>
{
    private readonly RequestHandler _requestHandler;
    public AccountOfficeController(ICrudService service, RequestHandler requestHandler) : base(service, "account_office")
    {
        _requestHandler = requestHandler;
    }
    [HttpPost]
    [Route("LinkAccountToOffice")]
    public ResponseDto LinkAccountToOffice([FromBody] CreateAccountOfficeDto dto)
    {
        return ValidateAndProceed(() => Service.CreateItemWithoutReturn(TableName,
                new Dictionary<string, object>
                {
                    { "account_id", dto.Account_id },
                    { "office_id", dto.Office_id },
                    { "account_rank", dto.Account_rank }
                }), 
            $"Linked account to office.");
    }

    [HttpPut]
    [Route("GetAccountsForOffice/{id:int}")]
    public ResponseDto GetAccountsForOffice([FromRoute] int id)
    {
        return ValidateAndProceed(() => Service.GetItemsByParameters<GetAccountOfficeDto>(TableName,
            new Dictionary<string, object>{ { "office_id", id } }), 
            $"Fetched all accounts for office: {id}");
    }
    
    [HttpPut]
    [Route("GetOfficesForAccount/{id:int}")]
    public ResponseDto GetOfficesForAccount([FromRoute] int id)
    {
        return ValidateAndProceed(() => Service.GetItemsByParameters<GetAccountOfficeDto>(TableName,
                new Dictionary<string, object>{ { "account_id", id } }), 
            $"Fetched all offices for account: {id}");
    }

    [HttpDelete]
    [Route("RemoveAccountFromOffcie")]
    public ResponseDto RemoveAccountFromOffice([FromBody] DeleteAccountOfficeDto dto)
    {
        return ValidateAndProceed(() => Service.DeleteItemWithMultipleParams(TableName,
            new Dictionary<string, object>
            {
                { "account_id", dto.Account_id },
                { "office_id", dto.Office_id }
            }), "Removed account from office.");
    }

    [HttpPut]
    [Route("ModifyAccountRank")]
    public ResponseDto ModifyAccountRankForOffice([FromBody] UpdateAccountOfficeDto dto)
    {
        return ValidateAndProceed(() => Service.UpdateItem(TableName,
            new Dictionary<string, object>
            {
                { "account_id", dto.Account_id },
                { "office_id", dto.Office_id }
            }, new Dictionary<string, object>
            {
                { "account_rank", dto.Account_rank }
            }), "Updated account rank.");
    }
}