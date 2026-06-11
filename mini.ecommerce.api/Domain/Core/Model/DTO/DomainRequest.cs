namespace mini.ecommerce.api.Domain.Core.Model.DTO
{
    public class DomainRequest<T>
    {
        public T request { get; set; }

        public DomainRequest(T request)
        {
            this.request = request;
        }
    }
}
