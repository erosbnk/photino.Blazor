using Photino.NET;

namespace Photino.Blazor;

public partial class PhotinoBlazorApp(IHost host)
{
    internal IHost Host { get; } = host;

    public IHostEnvironment Environment => Services.GetRequiredService<IHostEnvironment>();
    public IHostApplicationLifetime Lifetime => Services.GetRequiredService<IHostApplicationLifetime>();
    public IServiceProvider Services => Host.Services;
    public PhotinoWindow Window => Services.GetRequiredService<PhotinoWindow>();
    public PhotinoWebViewManager WindowManager => Services.GetRequiredService<PhotinoWebViewManager>();

    private void ConfigureDefaults() => Window
        .SetTitle("Photino Blazor App")
        .SetUseOsDefaultSize(false)
        .SetUseOsDefaultLocation(false)
        .SetWidth(1000)
        .SetHeight(900)
        .SetLeft(450)
        .SetTop(100);

    private bool WindowClosingHandler(object sender, EventArgs e)
    {
        Lifetime.StopApplication();
        return false;
    }

    internal void Initialize()
    {
        Host.Start();
        Lifetime.ApplicationStopped.Register(Window.Close);

        ConfigureDefaults();
        Window.RegisterCustomSchemeHandler(PhotinoWebViewManager.BlazorAppScheme, HandleWebRequest);
        Window.RegisterWindowClosingHandler(WindowClosingHandler);

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
}