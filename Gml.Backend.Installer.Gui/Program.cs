using Gml.Backend.Installer.Gui;
using Terminal.Gui;

Application.Init();

try
{
    Application.Run(new MyView());
}
finally
{
    Application.Shutdown();
}