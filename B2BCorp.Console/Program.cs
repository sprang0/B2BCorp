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
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static ServiceProvider? serviceProvider;

    static void Main()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
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
}

    private static void ConfigureServices(ServiceCollection services)
    {
        // Managers
        services.AddScoped<ICustomerManager, CustomerManager>();
        services.AddScoped<IProductManager, ProductManager>();
        services.AddScoped<IOrderManager, OrderManager>();
        // Engines
        services.AddScoped<IOrderValidationEngine, OrderValidationEngine>();
        // RAs
        services.AddScoped<ICustomerRA, CustomerRA>();
        services.AddScoped<IOrderRA, OrderRA>();
        services.AddScoped<IProductRA, ProductRA>();
        services.AddScoped<ICustomerValidationRA, CustomerValidationRA>();
        services.AddScoped<IProductValidationRA, ProductValidationRA>();
        // DB Context
        services.AddDbContext<B2BDbContext>();
        // App
        services.AddSingleton<TestHarness, TestHarness>();
    }
}