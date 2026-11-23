using Reqnroll. BoDi;
namespace automation.Tests.Support;

public static class Dependencies
{
    public static void Register(IObjectContainer container)
    {
        container.RegisterTypeAs<PlaywrightDriver, PlaywrightDriver>(); 
    }
}