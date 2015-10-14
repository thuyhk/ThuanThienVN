using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Reflection;
using HGT.Entities.Models.Mapping;
using System.Linq;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class HGTContext : DataContext
    {
        static HGTContext()
        {
            Database.SetInitializer<HGTContext>(null);
        }

        public HGTContext()
            : base("Name=HGTContext")
        {
        }

        public DbSet<Banner> Banners { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<EmailQueue> EmailQueues { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<SolutionProductMapping> SolutionProductMappings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
        }
    }
}
