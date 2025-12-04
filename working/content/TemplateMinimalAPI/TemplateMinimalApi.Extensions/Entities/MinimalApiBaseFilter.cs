namespace TemplateMinimalApi.Extensions.Entities;

public abstract class MinimalApiBaseFilter
{
    public int? NumeroPagina { get; set; }
    public int? QuantidadePorPagina { get; set; }

    protected MinimalApiBaseFilter() { }

    public void VerificarDadosDePaginacao()
    {
        if (NumeroPagina is null || NumeroPagina <= 0)
            NumeroPagina = 0;

        if (QuantidadePorPagina is null || QuantidadePorPagina <= 0)
            QuantidadePorPagina = 30;

        if (QuantidadePorPagina > 50)
            QuantidadePorPagina = 50;
    }
}
