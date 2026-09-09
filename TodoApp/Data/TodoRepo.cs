using Dapper;
using MySqlConnector;
using smk_practical_project.Models;

namespace smk_practical_project.Data;

public class TodoRepo
{
    private readonly string _connectionString;

    public TodoRepo(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string not found."
            );
    }

    private MySqlConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }

    // Read, Create, Update, Delete operations 
    public async Task<IEnumerable<TodoItem>> GetAllAsync() // IEnumerable, TodoItem --> returns collection of TodoItem objects
    {
        using var connection = CreateConnection();

        const string sql = """
        SELECT Id, Title, IsCompleted
        FROM Todos
        ORDER BY Id;
        """;

        return await connection.QueryAsync<TodoItem>(sql); // QueryAsync --> convert each returned row into a TodoItem
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();

        const string sql = """
        SELECT Id, Title, IsCompleted
        FROM Todos
        Where Id = @Id;
        """;

        return await connection.QuerySingleOrDefaultAsync<TodoItem>(
            sql,
            new {Id = id});
    }

    public async Task<int> CreateAsync(string title)
    {
        using var connection = CreateConnection();

        const string sql = """
        INSERT INTO Todos (Title, IsCompleted)
        VALUES (@Title, false);
        
        SELECT LAST_INSERT_ID();
        """;

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new {Title = title});
    }

    public async Task UpdateAsync(TodoItem todo)
    {
        using var connection = CreateConnection();

        const string sql = """
        UPDATE Todos
        SET Title = @Title, IsCompleted = @IsCompleted
        WHERE Id = @Id;
        """;

        await connection.ExecuteAsync(sql, todo);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = CreateConnection();

        const string sql = """
        DELETE FROM Todos
        WHERE Id = @Id;
        """;

        await connection.ExecuteAsync(
            sql,
            new {Id = id});
    }
}