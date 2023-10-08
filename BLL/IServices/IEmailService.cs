namespace BLL.IServices;

public interface IEmailService
{
    Task SendVerifyUserAccountEmail(string to, string FirstName, string LastName, string code);
    Task SendVerifyCommerceAccountEmail(string to, string commerceName, string code);
}
