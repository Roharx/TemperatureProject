using api.Config;
using api.DTOs;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers;

    [Authorize]
    [Route("api/[controller]")]
    public class GenericControllerBase<TService, TDto, TCreateDto, TUpdateDto> : ControllerBase
        where TService : ICrudService
    {
        protected readonly TService Service;
        protected readonly string TableName;

        public GenericControllerBase(TService service, string tableName)
        {
            Service = service;
            TableName = tableName;
        }

        [NonAction]
        private static bool IsUrlAllowed(string url)
        {
            return Whitelist.AllowedUrls.Any(url.StartsWith);
        }

        [NonAction]
        private static ResponseDto HandleInvalidRequest()
        {
            return new ResponseDto
            {
                MessageToClient = "Invalid request.",
                ResponseData = null
            };
        }

        [NonAction]
        protected ResponseDto ValidateAndProceed<TResult>(Func<TResult> action, string successMessage)
        {
            try
            {
                var responseData = action.Invoke();
                return new ResponseDto
                {
                    MessageToClient = $"Successfully {successMessage}.", 
                    ResponseData = responseData
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ResponseDto { MessageToClient = "An error occurred while processing your request." };
            }
        }

        [HttpGet]
        [Authorize]
        [Route("getAll")]
        public virtual ResponseDto GetAllItems()
        {
            return ValidateAndProceed(() => Service.GetAllItems<TDto>(TableName), 
                "fetched all items");
        }

        [HttpGet]
        [Authorize]
        [Route("getById/{id:int}")]
        public virtual ResponseDto GetItemById([FromRoute] int id)
        {
            return ValidateAndProceed(() =>
                Service.GetSingleItemByParameters<TDto>(TableName, 
                    new Dictionary<string, object> { { "id", id } }), 
                $"fetched item with ID {id}");
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        public virtual ResponseDto CreateItem([FromBody] TCreateDto dto)
        {
            return ValidateAndProceed(() =>
            {
                var parameters = dto.GetType().GetProperties().ToDictionary(
                    prop => prop.Name, prop => prop.GetValue(dto, null));
                return TableName == "account" ? Service.CreateAccount(parameters) : 
                    Service.CreateItem<TDto>(TableName, parameters);
            }, "created item");
        }

        [HttpPut]
        [Authorize]
        [Route("update")]
        public virtual ResponseDto UpdateItem([FromHeader] int id, [FromBody] TUpdateDto dto)
        {
            return ValidateAndProceed<bool>(() =>
            {
                var modifications = dto.GetType().GetProperties().ToDictionary(
                    prop => prop.Name, prop => prop.GetValue(dto, null));
                var conditionColumns = new Dictionary<string, object> { { "id", id } };

                return TableName == "account" ? Service.UpdateAccount(conditionColumns, modifications) :
                    Service.UpdateItem(TableName, conditionColumns, modifications);
            }, "updated item");
        }


        [HttpDelete]
        [Authorize]
        [Route("delete/{id:int}")]
        public virtual ResponseDto DeleteItem([FromRoute] int id)
        {
            return ValidateAndProceed(() => Service.DeleteItem(TableName, id),
                "deleted item with given conditions");
        }
    }

