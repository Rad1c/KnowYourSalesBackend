using BLL.Enums;
using BLL.IServices;
using MODEL.Entities;

namespace BLL.Services;

public class SessionService : ISessionService
{
    private readonly IUserService _userService;
    private readonly IShopService _commerceService;
    public Guid? Id { get; private set; }
    public Guid? AccountId { get; private set; }
    public RoleEnum? Role { get; private set; }

    public SessionService(IUserService userService, IShopService commerceService)
    {
        _userService = userService;
        _commerceService = commerceService;
    }


    public async void SetSession(RoleEnum role, Guid id)
    {
        Role = role;

        if (role == RoleEnum.User)
        {
            User? user = await _userService.GetUserById(id);
            Id = user?.Id;
            AccountId = user?.Acc.Id;
        }
        else if (role == RoleEnum.Shop)
        {
            Commerce? commerce = await _commerceService.GetCommerceById(id);
            Id = commerce?.Id;
            AccountId = commerce?.Acc.Id;
        }
    }

}

