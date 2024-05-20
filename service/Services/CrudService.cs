using exceptions;
using infrastructure.DataModels;
using infrastructure.Interfaces;
using service.Interfaces;

namespace service.Services;

public class CrudService : ICrudService
{
    private readonly ICrudHandler _crudHandler;
    private readonly IHashService _hashService;
    private readonly ITokenService _tokenService;

    public CrudService(ICrudHandler crudHandler, IHashService hashService, ITokenService tokenService)
    {
        _crudHandler = crudHandler;
        _hashService = hashService;
        _tokenService = tokenService;
    }
    
    public IEnumerable<T> GetAllItems<T>(string tableName)
    {
        return _crudHandler.GetAllItems<T>(tableName);
    }

    public IEnumerable<T> GetItemsByParameters<T>(string tableName, Dictionary<string, object> parameters)
    {
        return _crudHandler.GetItemsByParameters<T>(tableName, parameters);
    }

    public T? GetSingleItemByParameters<T>(string tableName, Dictionary<string, object> parameters)
    {
        return _crudHandler.GetSingleItemByParameters<T>(tableName, parameters);
    }

    public IEnumerable<T> GetSelectedParametersForItems<T>(string tableName, string columns, Dictionary<string, object> parameters)
    {
        return _crudHandler.GetSelectedParametersForItems<T>(tableName, columns, parameters);
    }

    public int CreateItem<T>(string tableName, Dictionary<string, object> parameters)
    {
        return _crudHandler.CreateItem(tableName, parameters);
    }

    public bool CreateItemWithoutReturn(string tableName, Dictionary<string, object> parameters)
    {
        return _crudHandler.CreateItemWithoutReturn(tableName, parameters);
    }

    public bool UpdateItem(string tableName, Dictionary<string, object> conditionColumns, Dictionary<string, object> modifications)
    {
        return _crudHandler.UpdateItem(tableName, conditionColumns, modifications);
    }

    public bool DeleteItem(string tableName, int itemId)
    {
        return _crudHandler.DeleteItem(tableName, itemId);
    }

    public bool DeleteItemWithMultipleParams(string tableName, Dictionary<string, object> conditionColumns)
    {
        return _crudHandler.DeleteItemWithMultipleParams(tableName, conditionColumns);
    }

    public string? VerifyLogin(string username, string password)
    {
        try
        {
            var parameters = new Dictionary<string, object> { { "name", username } };
            var account = _crudHandler.GetSingleItemByParameters<AccountVerification>("account", parameters);
            return account != null && _hashService.VerifyPassword(
                account.Password, password) ? 
                _tokenService.GenerateToken(new Account(account.Id, account.Name, account.Email))
                : "Wrong username or password";
        }
        catch (Exception ex)
        {
            // Log the exception TODO: remove later
            Console.WriteLine("An error occurred during login verification: " + ex.Message);
            return "An error occurred during login verification";
        }
    }

    public int CreateAccount(Dictionary<string, object> accountData)
    {
        try
        {
            if (!accountData.ContainsKey("name") || 
                !accountData.ContainsKey("password") || 
                !accountData.ContainsKey("email"))
            {
                throw new Exceptions.InvalidAccountDataException(
                    "Username, password, and email are required.");
            }

            string password = accountData["password"].ToString();

            string hashedPassword;
            try
            {
                hashedPassword = _hashService.HashPassword(password);
            }
            catch (Exception ex)
            {
                throw new Exceptions.PasswordHashingException(
                    "An error occurred while hashing the password.", ex);
            }

            accountData["password"] = hashedPassword;

            return _crudHandler.CreateItem("account", accountData);
        }
        catch (Exception ex)
        {
            // Log the exception TODO: remove later
            Console.WriteLine("An error occurred during account creation: " + ex.Message);
            throw; // Re-throw the exception to be handled by the global exception handler
        }
    }
    
    public bool UpdateAccount(Dictionary<string, object> conditionColumns, Dictionary<string, object> modifications)
    {
        try
        {
            if (modifications.ContainsKey("Password"))
            {
                string password = modifications["Password"].ToString();

                string hashedPassword;
                try
                {
                    hashedPassword = _hashService.HashPassword(password);
                }
                catch (Exception ex)
                {
                    throw new Exceptions.PasswordHashingException("An error occurred while hashing the password.", ex);
                }

                modifications["Password"] = hashedPassword;
            }

            return _crudHandler.UpdateItem("account", conditionColumns, modifications);
        }
        catch (Exception ex)
        {
            // Log the exception TODO: remove later
            Console.WriteLine("An error occurred during account update: " + ex.Message);
            throw; // Re-throw the exception to be handled by the global exception handler
        }
    }
}
