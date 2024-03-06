using B2BCorp.Console;
using B2BCorp.Contracts.Engines.Validation;
using B2BCorp.Contracts.Managers.Customer;
using B2BCorp.Contracts.Managers.Product;
using B2BCorp.Contracts.ResourceAccessors.Customer;
using B2BCorp.Contracts.ResourceAccessors.Product;
using B2BCorp.CustomerManagers;
using B2BCorp.CustomerRAs;
using B2BCorp.DataModels;
using B2BCorp.ProductManagers;
using B2BCorp.ProductRAs;
using B2BCorp.ValidationEngines;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

class Program
{
    static ServiceProvider? serviceProvider;

    static void Main()
    {
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = configBuilder.Build();

        var services = new ServiceCollection();
        ConfigureServices(services, configuration);
        serviceProvider = services.BuildServiceProvider();

        MainAsync().Wait();

        Console.WriteLine("All done!");
    }

    static async Task MainAsync()
    {
        var app = serviceProvider!.GetRequiredService<TestHarness>();

        await app.CreateData();
        await app.LoadData();
        await app.PlaceExcessiveOrders();
        await app.PlaceInvalidOrders();
        await app.PlaceGoodOrder();
        await app.PlaceOrderBeyondCreditLimit();
        await app.UpdateProductsConcurrently();
}

    private static void ConfigureServices(ServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(_ => configuration);
        // Managers
        services.AddTransient<ICustomerSearchManager, CustomerSearchManager>();
        services.AddTransient<ICustomerEditManager, CustomerEditManager>();
        services.AddTransient<IProductSearchManager, ProductSearchManager>();
        services.AddTransient<IProductEditManager, ProductEditManager>();
        services.AddTransient<ICustomerOrderManager, CustomerOrderManager>();
        // Engines
        services.AddTransient<IOrderValidationEngine, OrderValidationEngine>();
        // RAs
        services.AddTransient<ICustomerSearchRA, CustomerSearchRA>();
        services.AddTransient<ICustomerEditRA, CustomerEditRA>();
        services.AddTransient<ICustomerOrderRA, CustomerOrderRA>();
        services.AddTransient<IProductSearchRA, ProductSearchRA>();
        services.AddTransient<IProductEditRA, ProductEditRA>();
        services.AddTransient<ICustomerValidationRA, CustomerValidationRA>();
        services.AddTransient<IProductValidationRA, ProductValidationRA>();
        // DB Context
        services.AddDbContext<B2BDbContext>
            (opt => opt.UseSqlServer(configuration!.GetConnectionString("DefaultConnection")));
        // App
        services.AddSingleton<TestHarness, TestHarness>();
    }
}