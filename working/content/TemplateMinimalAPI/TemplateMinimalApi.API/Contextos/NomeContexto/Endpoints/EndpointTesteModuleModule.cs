namespace TemplateMinimalApi.API.Contextos.NomeContexto.Endpoints;

public class EndpointTesteModule(IConfiguration configuration) : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var hasAuthentication = bool.Parse(configuration["BaseConfiguration:TemAutenticacao"]);
        var versionamento = CarterVersionExtensions.VersionEndpoints(app);

        #region Listagem de Parcelas de Contas a Receber [Authorize()]
        app.MapGet("/v{version:apiVersion}/endpoint-exemplo", async (IApiCustomResults customResults,
                             IMediator mediator) =>

        {
            var commandResult = new CommandResult("Estou vivo", true);

            return customResults.FormatApiResponse(commandResult, "");

        })
        .Produces<string>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
        .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
        .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
        .WithName("Endpoint de teste")
        .WithTags("Teste - Meu Endpoint")
        .WithDescription(@" <ul>
  <li><b>Filtro 1</b> -descrição de filtros caso tenha(<b>não obrigatório</b>)</li>
  <li><b>NumeroPagina</b> - número da página de pesquisa (se não informado, o valor é <b>1</b>)</li>
  <li><b>QuantidadePorPagina</b> - quantidade de registros por página( se não informado, o valor retornado será de <b>30 registros</b>.)</li>
  <li>A <b>paginação máxima</b> permitida é <b>50 registros</b>.</li>
                            </ul>
     &#09;O Retorno sempre será no padrão <b>CommandResult</b>: <br/>
                            {
                              ""<b>success</b>"": 'Mostra se a requisição foi sucesso ou não',
                              ""<b>message</b>"": 'Mensagem relacionada ao status da requisição',
                              ""<b>data</b>"": 'Conteúdo da requisição( No caso de erros, será um objeto do tipo <b>ProblemDetails</b>)'
                             }"
)
        .WithSummary("Endpoint de Teste de aplicação")
        .WithOpenApi()
        .RequireAuthorizationConditionally(hasAuthentication)
        .WithApiVersionSet(versionamento)
        .MapToApiVersion(1);
        #endregion
    }
}
