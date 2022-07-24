using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HBDStack.EfCore.Hooks;
using HBDStack.EfCore.Hooks.Internals;
using HBDStack.EfCore.HookTests.Data;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HBDStack.EfCore.HookTests.Hooks;

public class HookTests : IClassFixture<HookFixture>
{
    private readonly ServiceProvider _provider;

    public HookTests(HookFixture fixture) => _provider = fixture.Provider;

    // [Fact]
    // public void TestAddHook()
    // {
    //     var hook = _provider.GetRequiredService<Hook>();
    //     var db = _provider.GetRequiredService<HookContext>();
    //
    //     db.Set<Profile>().Add(new Profile { Name = "Duy" });
    //     db.SaveChanges();
    //
    //     hook.BeforeCalled.Should().BeTrue();
    //     hook.AfterCalled.Should().BeTrue();
    // }

    [Fact]
    public async Task TestAddHookAsync()
    {
        var hook = _provider.GetRequiredService<Hook>();
        hook.Reset();

        var db = _provider.GetRequiredService<HookContext>();

        db.Set<Profile>().Add(new Profile { Name = "Duy" });
        await db.SaveChangesAsync().ConfigureAwait(false);

        hook.BeforeCalled.Should().BeTrue();
        hook.AfterCalled.Should().BeTrue();
    }

    [Fact]
    public async Task TestCallSaveChangesTwiceAsync()
    {
        var hook = _provider.GetRequiredService<Hook>();
        hook.Reset();
        var db = _provider.GetRequiredService<HookContext>();

        db.Set<Profile>().Add(new Profile { Name = "Duy" });
        await db.SaveChangesAsync().ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);

        hook.BeforeCalled.Should().BeTrue();
        hook.AfterCalled.Should().BeTrue();
    }

    [Fact]
    public async Task HookShouldNotRaise()
    {
        var hook = _provider.GetRequiredService<Hook>();
        hook.Reset();
        var db = _provider.GetRequiredService<HookContext>();
        var disabled = db.DisposableHooks(hook);

        //Check global flag
        HookExtensions.DisabledHooks.Count.Should().Be(1);

        db.Set<Profile>().Add(new Profile { Name = "Duy" });
        await db.SaveChangesAsync().ConfigureAwait(false);

        hook.BeforeCalled.Should().BeFalse();
        hook.AfterCalled.Should().BeFalse();

        //Check global flag after disposed.
        disabled.Dispose();
        HookExtensions.DisabledHooks.Count.Should().Be(0);
    }

    [Fact]
    public void AddHookRunnerTwice()
    {
        var services = new ServiceCollection()
            .AddHookRunner()
            .AddHookRunner();

        services.Count(s => s.ServiceType == typeof(HookRunner))
            .Should().Be(1);
    }

    [Fact]
    public void AddHookTwice()
    {
        var services = new ServiceCollection()
            .AddHook<Hook>()
            .AddHook<Hook>();

        services.Count(s => s.ImplementationType == typeof(Hook))
            .Should().Be(1);
    }
}