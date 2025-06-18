namespace TemplateMinimalApi.Extensions.Shared.Configurations;

public record EmailConfigurationOptions
{
    public const string EmailConfig = "EmailConfiguration";

    public string? Remetente { get; set; }
    public string? Destinatario { get; set; }
    public string? Senha { get; set; }
    public int Porta { get; set; }
    public string? SMTP { get; set; }

    public EmailConfigurationOptions() { }

}
