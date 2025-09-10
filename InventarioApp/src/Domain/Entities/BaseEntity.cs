namespace InventarioApp.src.Domain.Entities
{
    public abstract class BaseEntity
    {
        public string Id { get; }

        public BaseEntity(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id requerido");
            Id = id;
        }
    }
}
