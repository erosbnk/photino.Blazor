using HelloWorld.Components;
using Microsoft.AspNetCore.Components.Web;
using Photino.Blazor;
using System;

namespace HelloWorld;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        var builder = PhotinoBlazorAppBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        var app = builder.Build();

        app.Window.SetIconFile("favicon.ico")
                  .SetTitle("Photino Blazor Sample")
                  .SetNotificationsEnabled(false);

        AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
        {
            app.Window.ShowMessage("Fatal exception", error.ExceptionObject.ToString());
        };

        app.Run();
    }
}