using api.DTOs;
using api.DTOs.Account;
using api.DTOs.Office;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;

namespace api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class OfficeController : GenericControllerBase<ICrudService, GetOfficeDto, CreateOfficeDto, UpdateOfficeDto>
{
    private readonly RequestHandler _requestHandler;
    public OfficeController(ICrudService service, RequestHandler requestHandler) : base(service, "office")
    {
        _requestHandler = requestHandler;
    }
    
    [HttpPut]
    [Route("getByName")]
    public ResponseDto GetOfficeByName([FromBody] string name)
    {
        return ValidateAndProceed(
            () => Service.GetSingleItemByParameters<GetOfficeDto>(TableName, 
                new Dictionary<string, object> { { "name", name } }), 
            $"fetched office with name {name}");
    }
}