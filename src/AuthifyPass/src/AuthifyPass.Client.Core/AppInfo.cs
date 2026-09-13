using System.Reflection;

namespace AuthifyPass.Client.Core;

public static class AppInfo
{
    // Release label resolved at build time from the assembly informational version, which is derived
    // from the git commit count by the SetVersionFromGit target in Directory.Build.props. Nothing to
    // hand-edit. The service worker's published APP_BUILD uses the same commit count (injected at publish).
    public static string Version { get; } = ResolveVersion();

    private static string ResolveVersion()
    {
        AssemblyInformationalVersionAttribute? informationalVersion = typeof(AppInfo).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>();

        string result = informationalVersion?.InformationalVersion ?? "0.0";

        // Strip any trailing "+<sha>" the SDK might still append on some hosts.
        int plusIndex = result.IndexOf('+');
        if (plusIndex >= 0)
        {
            result = result[..plusIndex];
        }

        return result;
    }
}
