using Photino.NET;

namespace Photino.Blazor;

public partial class PhotinoBlazorApp(IHost host)
{
    private readonly IHost _host = host;

    public IHostEnvironment Environment => Services.GetRequiredService<IHostEnvironment>();
    public IHostApplicationLifetime Lifetime => Services.GetRequiredService<IHostApplicationLifetime>();
    public IServiceProvider Services => _host.Services;
    public PhotinoWindow Window => Services.GetRequiredService<PhotinoWindow>();
    public PhotinoWebViewManager WindowManager => Services.GetRequiredService<PhotinoWebViewManager>();

    public Stream HandleWebRequest(object? sender, string? scheme, string url, out string contentType)
        => WindowManager.HandleWebRequest(sender, scheme, url, out contentType!)!;

    public void Run()
    {
        if (string.IsNullOrWhiteSpace(Window.StartUrl))
        {
            Window.StartUrl = "/";
        }

        WindowManager.Navigate(Window.StartUrl);
        Window.WaitForClose();
    }

    internal void Initialize()
    {
        _host.Start();
        Lifetime.ApplicationStopped.Register(Window.Close);

        ConfigureDefaults();
        Window.RegisterCustomSchemeHandler(PhotinoWebViewManager.BlazorAppScheme, HandleWebRequest);

        var windowManager = Services.GetRequiredService<PhotinoWebViewManager>();
        var rootComponents = Services.GetRequiredService<PhotinoRootComponentsList>();

        foreach (var component in rootComponents)
        {
            _ = windowManager.Dispatcher.InvokeAsync(async () =>
            {
                await windowManager.AddRootComponentAsync(component.ComponentType, component.Selector, component.Parameters);
            });
        }
    }

    private void ConfigureDefaults() => Window
        .SetTitle("Photino Blazor App")
        .SetUseOsDefaultSize(false)
        .SetUseOsDefaultLocation(false)
        .SetWidth(1000)
        .SetHeight(900)
        .SetLeft(450)
        .SetTop(100);
}