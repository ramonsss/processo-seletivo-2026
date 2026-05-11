using apiMcardPrePagoGestorRelatorio.Domain.Core.Enums;
using System.Text.Json;

namespace apiMcardPrePagoGestorRelatorio.Domain.Core.Base
{
    public record BaseError
    {
        public EnumStatus? tipoErro { get; init; }
        public int? codErro { get; init; }
        public string? msgErro { get; init; }
        public string? origemErro { get; init; }

        public BaseError(EnumStatus tipoErro, int? codErro, string? msgErro)
        {
            this.tipoErro = tipoErro;
            this.codErro = codErro;
            this.msgErro = msgErro;
            this.origemErro = "apiMardPrePagoPortadorCartao";
        }

        public BaseError(EnumStatus tipoErro, int? codErro, string? msgErro, string? origemErro)
        {
            this.tipoErro = tipoErro;
            this.codErro = codErro;
            this.msgErro = msgErro;
            this.origemErro = origemErro;
        }

        public BaseError(string json)
        {
            tipoErro = EnumStatus.NEGOCIO;
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                codErro = root.TryGetProperty("codErro", out var codElement) && int.TryParse(codElement.GetString(), out var codigo) ? codigo : null;
                msgErro = root.TryGetProperty("msgErro", out var msgElement) ? msgElement.GetString() : null;
            }
            catch (JsonException)
            {
                codErro = null;
                msgErro = json;
            }
        }
    }
}

