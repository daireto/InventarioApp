namespace InventarioApp.src.Domain.Entities
{
    public abstract class Product : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        private int _quantity;

        public int Quantity
        {
            get => _quantity;
            private set => _quantity = value >= 0 ? value : 0;
        }

        protected Product(string id, string name, string description, decimal price, int quantity) : base(id)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nombre requerido");
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Descripción requerida");
            if (price < 0) throw new ArgumentException("El precio no puede ser negativo");
            if (quantity < 0) throw new ArgumentException("La cantidad no puede ser negativa");
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException("Nombre requerido");
            Name = newName;
        }

        public void UpdateDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription)) throw new ArgumentException("Descripción requerida");
            Description = newDescription;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0) throw new ArgumentException("El precio no puede ser negativo");
            Price = newPrice;
        }

        public void IncreaseStock(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("La cantidad debe ser positiva");

            _quantity += amount;
        }

        public void DecreaseStock(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("La cantidad debe ser positiva");

            if (_quantity < amount)
                throw new InvalidOperationException("No hay stock disponible");

            _quantity -= amount;
        }

        public virtual string GetDisplayInfo()
        {
            return $"ID: {Id}, Nombre: {Name}, Precio: ${Price:C}, Cantidad: {Quantity}";
        }

        public virtual string GetNameAndStock()
        {
            return $"{Name} (Cantidad: {Quantity})";
        }

        public virtual string GetCSVLine()
        {
            return $"{Id},{Name},{Description},{Price},{Quantity},Genérico";
        }
    }

    public sealed class PerishableProduct : Product
    {
        public DateTime ExpirationDate { get; private set; }

        public PerishableProduct(string id, string name, string description, decimal price, int quantity, DateTime expirationDate)
            : base(id, name, description, price, quantity)
        {
            if (expirationDate <= DateTime.Now)
                throw new ArgumentException("La fecha de expiración debe ser futura", nameof(expirationDate));
            ExpirationDate = expirationDate;
        }

        public override string GetDisplayInfo()
        {
            return $"{base.GetDisplayInfo()}, Expira en: {ExpirationDate.ToShortDateString()}";
        }

        public override string GetNameAndStock()
        {
            return $"{Name} (Perecedero) (Cantidad: {Quantity})";
        }

        public override string GetCSVLine()
        {
            return $"{Id},{Name},{Description},{Price},{Quantity},Perecedero";
        }
    }

    public sealed class NonPerishableProduct : Product
    {
        public string Category { get; private set; }

        public NonPerishableProduct(string id, string name, string description, decimal price, int quantity, string category)
            : base(id, name, description, price, quantity)
        {
            if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Categoría requerida");
            Category = category;
        }

        public override string GetDisplayInfo()
        {
            return $"{base.GetDisplayInfo()}, Categoría: {Category}";
        }

        public override string GetNameAndStock()
        {
            return $"{Name} (No perecedero) (Cantidad: {Quantity})";
        }

        public override string GetCSVLine()
        {
            return $"{Id},{Name},{Description},{Price},{Quantity},No perecedero";
        }
    }
}
