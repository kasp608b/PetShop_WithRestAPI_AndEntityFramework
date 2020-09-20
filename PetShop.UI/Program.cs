using System;
using Microsoft.Extensions.DependencyInjection;
using PetShop.Core.ApplicationService;
using PetShop.Core.ApplicationService.Implementations;
using PetShop.Core.DomainService;
using PetShop.Infrastructure.Data;
using PetShop.Core.HelperClasses.Implementations;
using PetShop.Core.HelperClasses.Interfaces;

namespace PetShop.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            FakeDB.InitData();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IPetRepository, PetRepository>();
            serviceCollection.AddScoped<IPetService, PetService>();
            serviceCollection.AddScoped<IPrinter, Printer>();
            serviceCollection.AddScoped<IParser, Parser>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var printer = serviceProvider.GetRequiredService<IPrinter>();
            printer.StartMenu();
        }
    }
}
