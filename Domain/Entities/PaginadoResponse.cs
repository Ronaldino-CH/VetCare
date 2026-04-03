namespace Domain.Entities
{
    public class PaginadoResponse<T>
    {
        public ICollection<T> Data { get; set; }
        public Meta Meta { get; set; }

        public PaginadoResponse(ICollection<T>? data, Meta meta)
        {
            Data = data ?? new List<T>();
            Meta = meta;
        }
    }
}
