namespace TemplateMinimalApi.Extensions.Shared.Services;
public interface IEmailServices
{
    Task EnviarEmailAsync(EmailInfo emailInfo);
}
