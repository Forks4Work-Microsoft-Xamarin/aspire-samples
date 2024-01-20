System.Diagnostics.Process.Start
                            (
                                "/Users/moljac/Library/Android/sdk/emulator/emulator",
                                "-avd Pixel_3a_API_34_extension_level_7_arm64-v8a"
                                // https://developer.android.com/studio/run/emulator-commandline
                                + " " +
                                "-no-cache" 
                                + " " +
                                "-gpu on"
                                + " " +
                                "-no-snapshot-load"
                                + " " +
                                "-no-boot-anim"
                            );
System.Threading.Thread.Sleep(10000);

var builder = DistributedApplication.CreateBuilder(args);

var catalogDb = builder.AddPostgresContainer("catalog").AddDatabase("catalogdb");
var basketCache = builder.AddRedisContainer("basketcache");

var catalogService = builder.AddProject<Projects.eShopLite_CatalogService>("catalogservice")
    .WithReference(catalogDb);

var basketService = builder.AddProject<Projects.eShopLite_BasketService>("basketservice")
    .WithReference(basketCache);

builder.AddProject<Projects.eShopLite_Frontend>("frontend")
    .WithReference(basketService)
    .WithReference(catalogService);

builder.AddProject<Projects.eShopLite_CatalogDbManager>("catalogdbmanager")
    .WithReference(catalogDb);

System.Diagnostics.Process.Start
                            (
                                "open",
                                "-a \"Microsoft Edge\" -- http://localhost:15178"
                            );

/*
*/
builder.AddProject("frontend_client_maui", "..\\eShopLite.AppMAUI\\eShopLite.AppMAUI.csproj")
    .WithReference(basketService)
    .WithReference(catalogService);

builder
    .BuildMAUI("android","Pixel_3a_API_34_extension_level_7_arm64-v8a")
    .BuildMAUI("ios","8DD23CF2-C0C4-4A5C-928C-4C8AC83EE8D0")
    .BuildMAUI("maccatalyst")
    ;
    
builder
    .BuildDistributedAppWithClientsMAUI()    // injected MAUI + builder.Build();
    .Run()
    ;



public static partial class IDistributedApplicationBuilderExtensions
{
    private static IDistributedApplicationBuilder? b;
    private static List<(string tfm, string device)> devices = new List<(string tfm, string device)>();
     
    public static
        IDistributedApplicationBuilder
                                        BuildMAUI(this IDistributedApplicationBuilder? builder, string tfm, string? device = null)
    {
        Console.WriteLine("mc++");
        Console.WriteLine("     Inside Extension Method");
        Console.WriteLine("         BuildMAUI");

        b = builder;
        devices.Add((tfm, device));
        
        return builder;
    }

    public static
        DistributedApplication
                                        BuildDistributedAppWithClientsMAUI(this IDistributedApplicationBuilder? builder)
    {
        Console.WriteLine("mc++");
        Console.WriteLine("     Inside Extension Method");
        Console.WriteLine("         RunMAUI");

        foreach (var r in b.Resources)
        {
            string n = r.Name;
        }

        System.Diagnostics.Process.Start
        (
            "dotnet",
            "build -f:net8.0-maccatalyst -t:run ..\\\\eShopLite.AppMAUI\\\\eShopLite.AppMAUI.csproj"
        );
        System.Diagnostics.Process.Start
        (
            "dotnet",
            "build -f:net8.0-ios -t:run ..\\\\eShopLite.AppMAUI\\\\eShopLite.AppMAUI.csproj"
        );
        System.Diagnostics.Process.Start
        (
            "dotnet",
            "build -f:net8.0-android -t:run ..\\\\eShopLite.AppMAUI\\\\eShopLite.AppMAUI.csproj"
        );

        Parallel.ForEach
        (
            devices,
            tuple =>
            {
            }
        );

        return builder.Build();   // can be called only once
    }
}
