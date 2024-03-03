using Microsoft.EntityFrameworkCore;

namespace B2BCorp.DataModels
{
    public class B2BDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public string DbPath { get; }

        public B2BDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "B2BCorp.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
            options.UseSqlite($"Data Source={DbPath}");

        public class Customer
        {
            // PK
            public Guid CustomerId { get; set; }

            // Fields
            public string Name { get; set; } = "";
            public bool IsVerified { get; set; }
            public decimal CreditLimit { get; set; }

            // Refs
            public List<Order> Orders { get; } = [];
            public List<Invoice> Invoices { get; } = [];
        }

        public class Product
        {
            // PK
            public Guid ProductId { get; set; }

            // Fields
            public string Name { get; set; } = "";
            public decimal Price { get; set; }
            public bool IsActivated { get; set; }
            public bool IsDiscontinued { get; set; }
            public int MinAllowedPerOrder { get; set; }
            public int MaxAllowedPerOrder { get; set; }

            // Refs
            public List<OrderItem> OrderItems { get; set; } = [];
        }

        public class Order
        {
            // PK
            public Guid OrderId { get; set; }

            // Fields
            public bool RequiresReview { get; set; }
            public decimal TotalPrice { get; set; }
            public bool IsApproved { get; set; }
            public DateTime? OrderDateTime { get; set; }

            // FK
            public Guid CustomerId { get; set; }
            public Customer Customer { get; set; } = new();

            // Refs
            public List<OrderItem> OrderItems { get; } = [];
            public List<Invoice> Invoices { get; } = [];
        }

        public class OrderItem
        {
            // PK
            public Guid OrderItemId { get; set; }

            // Fields
            public int QuantityOrdered { get; set; }
            public decimal ExtendedPricePerItem { get; set; }
            public decimal TotalPrice { get; set; }

            // PK, FK
            public Guid OrderId { get; set; }
            public Order Order { get; set; } = new();
            public Guid ProductId { get; set;}
            public Product Product { get; set; } = new();
        }

        public class Invoice
        {
            // PK
            public Guid InvoiceId { get; set; }

            // Fields
            public decimal AmountDue { get; set; }
            public DateTime InvoiceDateTime { get; set; }
            public bool IsPaidInFull { get; set; }

            // FK
            public Guid CustomerId { get; set; }
            public Customer Customer { get; set; } = new();
            public Guid OrderId { get; set; }
            public Order Order { get; set; } = new();

            // Refs
            public List<Payment> Payments { get; } = [];
        }

        public class Payment
        {
            // PK
            public Guid PaymentId { get; set; }

            // Fields
            public decimal AmountPaid { get; set; }
            public DateTime PaymentDateTime { get; set; }

            // FK
            public Guid InvoiceId { get; set;}
            public Invoice Invoice { get; set; } = new();
        }
    }
}
