using Aspire.Hosting;

using HolisticWare.Aspire.Hosting.Maui;

HolisticWare.Tools.Devices.Android.Emulator.Launch("nexus_9_api_33");
HolisticWare.Tools.Devices.Android.Emulator.Launch("Pixel_3a_API_34_extension_level_7_arm64-v8a");
    
System.Threading.Thread.Sleep(10000);

string project_maui = "..\\eShopLite.AppMAUI\\eShopLite.AppMAUI.csproj";

project_maui = "..\\..\\..\\..\\..\\dotnet-architecture\\eshop-mobile-client\\m\\eShopOnContainers\\eShopOnContainers.csproj";
IDistributedApplicationBuilderMauiExtensions.ProjectMaui = project_maui;

var builder = DistributedApplication.CreateBuilder(args);

var catalogDb = builder
                                                        .AddPostgresContainer("catalog")
                                                        .AddDatabase("catalogdb");
var basketCache = builder
                                                        .AddRedisContainer("basketcache");

var catalogService = builder
                                                        .AddProject<Projects.eShopLite_CatalogService>("catalogservice")
                                                        .WithReference(catalogDb);

var basketService = builder
                                                        .AddProject<Projects.eShopLite_BasketService>("basketservice")
                                                        .WithReference(basketCache);


/*
*/
builder
    .AddProject<Projects.eShopLite_Frontend>("frontend")
    .WithReference(basketService)
    .WithReference(catalogService);

builder
    .AddProject<Projects.eShopLite_CatalogDbManager>("catalogdbmanager")
    .WithReference(catalogDb);


/*
*/
builder
    .AddProject
            (
                "frontend_client_maui", 
                project_maui,
                "net8.0-android",
                "Pixel_3a_API_34_extension_level_7_arm64-v8a",
                2
            )
    .WithReference(basketService)
    .WithReference(catalogService);

builder
    .BuildClient
            (
                "net8.0-android",
                "Pixel_3a_API_34_extension_level_7_arm64-v8a",
                2
            )
    .BuildClient
            (
                "net8.0-android",
                "Nexus_9_API_33"    // tablet
                // default = 1
            )
    .BuildClient
            (
                "net8.0-ios",
                "8DD23CF2-C0C4-4A5C-928C-4C8AC83EE8D0",                
                2
            )
    .BuildClient
            (
                "net8.0-ios",
                "4066B4FA-CCEF-4F1D-ABEA-BDE0E1471E33"  // iPad
            )
    .BuildClient("maccatalyst")
    ;
    
builder
    .BuildDistributedAppWithClientsMAUI()    // injected MAUI + builder.Build();
    .Run()
    ;
