using BLL.Common;
using BLL.Extensions;
using BLL.IServices;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace BLL.Services;

internal class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task SendVerifyUserAccountEmail(string to, string firstName, string LastName, string code)
    {
        EmailConfiguration emailConfig = _configuration.GetEmailConfiguration();

        if (!emailConfig.IsSendingEnabled) return Task.CompletedTask;

        string subject = $"Hi {firstName} {LastName}, please verify your {EmailConstants.CompanyName} account";
        string body = GetSendVerifyAccountBody(code);

        var client = new SmtpClient(emailConfig.SMTP, emailConfig.Port)
        {
            EnableSsl = emailConfig.EnableSSL,
            Credentials = new NetworkCredential(emailConfig.From, emailConfig.Password)
        };

        MailMessage message = new(
            from: emailConfig.From,
            to: to,
            subject: subject,
            body: body);

        message.IsBodyHtml = true;

        return client.SendMailAsync(message);
    }

    public Task SendVerifyCommerceAccountEmail(string to, string commerceName, string code)
    {
        EmailConfiguration emailConfig = _configuration.GetEmailConfiguration();

        if (!emailConfig.IsSendingEnabled) return Task.CompletedTask;

        string subject = $"Hi {commerceName}, please verify your {EmailConstants.CompanyName} account";
        string body = GetSendVerifyAccountBody(code);

        var client = new SmtpClient(emailConfig.SMTP, emailConfig.Port)
        {
            EnableSsl = emailConfig.EnableSSL,
            Credentials = new NetworkCredential(emailConfig.From, emailConfig.Password)
        };

        MailMessage message = new(
            from: emailConfig.From,
            to: to,
            subject: subject,
            body: body);

        message.IsBodyHtml = true;

        return client.SendMailAsync(message);
    }

    private static string GetSendVerifyAccountBody(string code)
    {
        string template = EmailTemplates.VerifyAccount;

        return template.Replace(EmailConstants.VerifyAccountPlaceholder, EmailConstants.VerifyAccountlUrl + $"/{code}");
    }
}
