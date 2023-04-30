using BLL.Enums;

namespace BLL.IServices;

public interface ISessionService
{
    Guid? Id { get; }
    Guid? AccountId { get; }
    RoleEnum? Role { get; }

    void SetSession(RoleEnum role, Guid id);
}

