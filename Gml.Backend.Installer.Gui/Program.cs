using Gml.Backend.Installer.Gui;
using Terminal.Gui;
using Terminal.Gui.App;
using Terminal.Gui.Drawing;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

Application.Init();

try
{
    Application.Run(new MainView());
}
finally
{
    Application.Shutdown();
}