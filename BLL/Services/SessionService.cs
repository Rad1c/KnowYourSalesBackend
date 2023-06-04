using BLL.Enums;
using BLL.IServices;

namespace BLL.Services;

public class SessionService : ISessionService
{
    public Guid? Id { get; private set; }
    public Guid? AccountId { get; private set; }
    public RoleEnum? Role { get; private set; }

    public void SetSession(RoleEnum role, Guid id)
    {
        Role = role;
        Id = id;
    }

}

