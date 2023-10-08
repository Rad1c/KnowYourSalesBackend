using BLL.Common;
using Microsoft.Extensions.Configuration;

namespace BLL.Extensions;

public static class IEmailExtension
{
    public static EmailConfiguration GetEmailConfiguration(this IConfiguration configuration)
    {
        return new EmailConfiguration()
        {
            IsSendingEnabled = bool.Parse(configuration["Email:EnableSending"]),
            From = configuration["Email:From"],
            Password = configuration["Email:Password"],
            SMTP = configuration["Email:Smtp"],
            Port = int.Parse(configuration["Email:Port"]),
            EnableSSL = bool.Parse(configuration["Email:EnableSsl"]),
        };
    }
}
