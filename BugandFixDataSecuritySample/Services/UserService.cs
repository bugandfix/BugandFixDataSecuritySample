using BugandFixDataSecuritySample.Abstraction;
using BugandFixDataSecuritySample.Data;
using BugandFixDataSecuritySample.Models;
using Dapper;

namespace BugandFixDataSecuritySample.Services;

public class UserService : IUserService
{
    private readonly IEncryptionService _encryptionService;
    private readonly DapperContext _dapperContext;
    public UserService(IEncryptionService encryptionService, DapperContext dapperContext)
    {
        _encryptionService = encryptionService;
        _dapperContext = dapperContext;
    }

    public bool SaveUser(User user)
    {
        user.Password = _encryptionService.Encrypt(user.Password);
        var result = 0;
        const string query = "INSERT INTO Users (UserId,Name,Email,Password,Phone) VALUES (@UserId,@Name,@Email,@Password,@Phone)";
        using (var connection = _dapperContext.CreateConnection())
        {
            result = connection.Execute(query, user);
        }
        if (result > 0)
            return true;
        else return false;
    }

    public User GetUser(string id)
    {
        var user = new User();

        const string query = "SELECT * FROM Users WHERE UserId=@UserId";
        using (var connection = _dapperContext.CreateConnection())
        {
            user = connection.QueryFirstOrDefault<User>(query, new { UserId = id });


        }
        if (user != null)
            user.Password = _encryptionService.Decrypt(user.Password);

        return user;
    }
}
