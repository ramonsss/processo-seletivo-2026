using System.Text.Json.Serialization;
using apiMcardPrePagoGestorRelatorio.Domain.Core.Enums;

namespace apiMcardPrePagoGestorRelatorio.Domain.Core.Base
{
    public struct BaseReturn<TSuccesso>
    {
        public EnumStatus Status { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TSuccesso? SuccessObject { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BaseError? ErrorObject { get; init; }


        [JsonIgnore]
        public bool IsSuccess => Status == EnumStatus.SUCESSO;


        private BaseReturn(EnumStatus status, TSuccesso? successObject, BaseError? errorObject)
        {
            Status = status;
            SuccessObject = successObject;
            ErrorObject = errorObject;
        }

        public static BaseReturn<TSuccesso> Success(TSuccesso value) =>
            new(EnumStatus.SUCESSO, value, null);
        public static BaseReturn<TSuccesso> BusinessError(BaseError error) =>
            new(EnumStatus.NEGOCIO, default, error);
        public static BaseReturn<TSuccesso> SystemError(BaseError error) =>
            new(EnumStatus.SISTEMA, default, error);
        public static BaseReturn<TSuccesso> Error(EnumStatus status, BaseError error) =>
            new(status, default, error);

        public BaseReturn<TSuccesso> Sucesso(TSuccesso mensagem) => Success(mensagem);
        public BaseReturn<TSuccesso> ErroNegocio(BaseError error) => BusinessError(error);
        public BaseReturn<TSuccesso> ErroSistema(BaseError error) => SystemError(error);
        public BaseReturn<TSuccesso> Erro(EnumStatus status, BaseError error) => Error(status, error);

        public static implicit operator BaseReturn<TSuccesso>(TSuccesso value) => Success(value);
        public static implicit operator BaseReturn<TSuccesso>(BaseError error) => SystemError(error);
    }
}