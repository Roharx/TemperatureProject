namespace infrastructure.Interfaces;

public interface ICrudHandler
{
    #region Read
    
    /// <summary>
    /// Retrieves all entries from a specified table.
    /// </summary>
    /// <param name="tableName">Name of the table</param>
    /// <typeparam name="T">The expected value (e.g., AccountQuery)</typeparam>
    /// <returns></returns>
    IEnumerable<T> GetAllItems<T>(string tableName);

    /// <summary>
    /// Retrieves all entries that have the specified parameters from a specified table.
    /// </summary>
    /// <param name="tableName">Name of the table</param>
    /// <param name="parameters">Can be one or multiple as well.</param>
    /// <typeparam name="T">The expected value (e.g., AccountQuery)</typeparam>
    /// <returns></returns>
    IEnumerable<T> GetItemsByParameters<T>(string tableName, Dictionary<string, object> parameters);

    /// <summary>
    /// Retrieves the first entry that has the specified parameters from a specified table.
    /// </summary>
    /// <param name="tableName">Name of the table</param>
    /// <param name="parameters">Parameters that the DB requires</param>
    /// <typeparam name="T">The expected value (e.g., AccountQuery)</typeparam>
    /// <returns></returns>
    T? GetSingleItemByParameters<T>(string tableName, Dictionary<string, object> parameters);

    /// <summary>
    /// Retrieves selected parameters from the given table inside the database that meet the specified conditions.
    /// </summary>
    /// <param name="tableName">Name of the table</param>
    /// <param name="columns">Columns to be selected (e.g., "Name, Email")</param>
    /// <param name="parameters">Parameters that the DB requires</param>
    /// <typeparam name="T">The expected value (e.g., AccountQuery)</typeparam>
    /// <returns></returns>
    IEnumerable<T> GetSelectedParametersForItems<T>(string tableName, string columns,
        Dictionary<string, object> parameters);
    
    #endregion

    #region Create


    /// <summary>
    /// Creates an item in a table of the database. It returns an integer ID.
    /// </summary>
    /// <param name="tableName">Name of the table</param>
    /// <param name="parameters">Parameters that the DB requires</param>
    /// <typeparam name="T">The expected value (e.g., id, AccountQuery, etc.)</typeparam>
    /// <returns></returns>
    int CreateItem(string tableName, Dictionary<string, object> parameters);

    /// <summary>
    /// Creates an item in a table of the database.
    /// </summary>
    /// <param name="tableName">Name of the table</param>
    /// <param name="parameters">Parameters that the DB requires</param>
    /// <typeparam name="T">The expected value (e.g., AccountQuery)</typeparam>
    /// <returns></returns>
    bool CreateItemWithoutReturn(string tableName, Dictionary<string, object> parameters);

    #endregion

    #region Update
    
    /// <summary>
    /// Modifies items in the specified table based on the given parameters and new values.
    /// <para>Usage example:</para>
    /// <para>var conditionColumns = new Dictionary&lt;string, object&gt;{</para>
    /// <para>    { "foreign_key1", 123 },</para>
    /// <para>    { "foreign_key2", 456 }</para>
    /// <para>};</para>
    /// <para>var modifications = new Dictionary&lt;string, object&gt;{</para>
    /// <para>    { "rank", 5 },</para>
    /// <para>    { "status", "active" }</para>
    /// <para>};</para>
    /// <para>x.UpdateItem("account", conditionColumns, modifications);</para>
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="conditionColumns">Dictionary containing conditions for the item selection based on column values.</param>
    /// <param name="modifications">Dictionary containing column names and their new values for the update.</param>
    /// <returns></returns>
    bool UpdateItem(string tableName, Dictionary<string, object> conditionColumns, Dictionary<string, object> modifications);

    #endregion

    #region Delete

    /// <summary>
    /// Removes an item from the table.
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="itemId"></param>
    /// <returns></returns>
    bool DeleteItem(string tableName, int itemId);
    
    /// <summary>
    /// Removes an item from the table. It allows multiple different parameter types.
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="conditionColumns"></param>
    /// <returns></returns>
    bool DeleteItemWithMultipleParams(string tableName, Dictionary<string, object> conditionColumns);
    
    #endregion
}