using FluentAssertions;
using HBDStack.EfCore.Hooks;
using HBDStack.EfCore.HookTests.Data;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HBDStack.EfCore.HookTests;

public class SetupTest
{
    [Fact]
    public void ServiceProvider_Setup_Test()
    {
        var provider = new ServiceCollection()
            .AddSingleton<Hook>()
            //.AddSingleton<IHook>(p => p.GetService<Hook>())
            .AddSingleton<IHookAsync>(p => p.GetService<Hook>())
            .BuildServiceProvider();

        //var instance1 = provider.GetService<IHook>();
        var instance2 = provider.GetService<IHookAsync>();

        //instance1.Should().NotBeNull();
        instance2.Should().NotBeNull();

        //instance1.Should().Be(instance2, "2 instances should be the same");
    }
}