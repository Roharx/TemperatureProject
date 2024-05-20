namespace service.Interfaces;

public interface ICrudService
{
    IEnumerable<T> GetAllItems<T>(string tableName);
    IEnumerable<T> GetItemsByParameters<T>(string tableName, Dictionary<string, object> parameters);
    T? GetSingleItemByParameters<T>(string tableName, Dictionary<string, object> parameters);
    IEnumerable<T> GetSelectedParametersForItems<T>(string tableName, string columns, Dictionary<string, object> parameters);

    int CreateItem<T>(string tableName, Dictionary<string, object> parameters);
    bool CreateItemWithoutReturn(string tableName, Dictionary<string, object> parameters);
    
    bool UpdateItem(string tableName, Dictionary<string, object> conditionColumns, Dictionary<string, object> modifications);

    bool DeleteItem(string tableName, int itemId);
    bool DeleteItemWithMultipleParams(string tableName, Dictionary<string, object> conditionColumns);

    string? VerifyLogin(string username, string password);
    int CreateAccount(Dictionary<string, object> accountData);
    bool UpdateAccount(Dictionary<string, object> conditionColumns, Dictionary<string, object> modifications);
}