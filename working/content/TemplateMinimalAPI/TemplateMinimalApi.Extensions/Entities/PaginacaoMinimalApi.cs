
namespace TemplateMinimalApi.Extensions.Entities;

public class PaginacaoMinimalApi<T> where T : class
{
    const int _TamanhoMaximoDePagina = 50;

    public int TotalDeItens { get; set; }
    public int PaginaAtual { get; set; }
    public int TamanhoDaPagina { get; set; }
    public int TotalDePaginas { get; set; }
    public bool ExisteProximaPagina { get; set; }
    public bool ExistePaginaAnterior { get; set; }
    public IEnumerable<T>? Resultado { get; set; }

    public PaginacaoMinimalApi(IEnumerable<T>? resultado, int totalDeItens, int paginaAtual, int tamanhoDaPagina)
    {
        Resultado = resultado;
        TotalDeItens = totalDeItens;
        PaginaAtual = paginaAtual;
        TamanhoDaPagina = tamanhoDaPagina;

        Resultado ??= new List<T>();

        if (PaginaAtual == 0)
            PaginaAtual = 1;

        ComputarDadosDaPaginacao();
    }

    private void ComputarDadosDaPaginacao()
    {
        TotalDePaginas = Convert.ToInt32(Math.Ceiling(((double)TotalDeItens / (double)TamanhoDaPagina)));

        ExisteProximaPagina = PaginaAtual < TotalDePaginas;
        ExistePaginaAnterior = PaginaAtual > 1;
    }

    public static int TratarQuantidadeDeRegistrosPorPagina(int? quantidade) => (quantidade.Value > 0 && quantidade.Value <= _TamanhoMaximoDePagina) ? quantidade.Value : _TamanhoMaximoDePagina;

    public static int TratarNumeroDePagina(int? numeroPagina, int? quantidade)
    {
        if (numeroPagina != 0)
            return (numeroPagina.Value - 1) * quantidade.Value;

        return 0;
    }

}
