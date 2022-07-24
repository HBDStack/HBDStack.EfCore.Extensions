using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.Hooks;

public static class HookExtensions
{
    internal static readonly List<(DbContext Context, dynamic Hook)> DisabledHooks = new();

    internal static bool IsHookDisabled(this DbContext context, dynamic hook) =>
        DisabledHooks.Any(d => Equals(d.Context, context) && Equals(d.Hook, hook));

    public static IDisposable DisposableHooks(this DbContext context, dynamic hook) =>
        new HookDisabledContext(context, hook);

    private class HookDisabledContext : IDisposable
    {
        private DbContext? _context;
        private dynamic? _hook;

        public HookDisabledContext(DbContext context, dynamic hook)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _hook = hook;

            DisabledHooks.Add((_context, _hook));
        }

        public void Dispose()
        {
            var index = DisabledHooks.FindIndex(d=> Equals(d.Context, _context) && Equals(d.Hook, _hook));
            if (index >= 0)
                DisabledHooks.RemoveAt(index);
            
            _context = null;
            _hook = null;
        }
    }
}