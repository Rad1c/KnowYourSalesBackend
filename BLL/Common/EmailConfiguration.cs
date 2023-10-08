namespace BLL.Common;

public sealed class EmailConfiguration
{
    public string From { get; set; } = string.Empty;
    public bool IsSendingEnabled { get; set; } = true;
    public string Password { get; set; } = string.Empty;
    public string SMTP { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool EnableSSL { get; set; } = true;
}
