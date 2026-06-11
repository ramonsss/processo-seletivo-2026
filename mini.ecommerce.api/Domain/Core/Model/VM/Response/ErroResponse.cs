namespace mini.ecommerce.api.Domain.Core.Model.VM.Response
{
    public record ErroResponse
    {
        public string? codigo { get; set; }
        public string? mensagem { get; set; }
        public List<Erros>? erros { get; set; }
        public string? origem { get; set; }

        public ErroResponse(string? codigo, string? mensagem, List<Erros>? erros = null, string? origem = "pix.automatico.gestao.agendamento.api")
        {
            this.codigo = codigo;
            this.mensagem = mensagem;
            this.erros = erros ?? [];
            this.origem = origem;
        }

        public ErroResponse(ErrorHttp erroHttp, string? origem)
        {
            codigo = erroHttp.codigo;
            mensagem = erroHttp.mensagem;
            erros = erroHttp.erros?.Select(e => new Erros(e.campo, e.mensagens)).ToList() ?? [];
            this.origem = origem ?? "desconhecida";
        }
    }
    
    public record Erros(
        string? campo,
        string[]? mensagens
    );

    public record struct ErrorHttp(
        string? codigo,
        string? mensagem,
        List<Erros>? erros
    )
    {
    }

    
}