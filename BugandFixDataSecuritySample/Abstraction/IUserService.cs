using BugandFixDataSecuritySample.Models;

namespace BugandFixDataSecuritySample.Abstraction;

public interface IUserService
{
    public User GetUser(string id);
    public bool SaveUser(User user);
}
