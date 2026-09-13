namespace AuthifyPass.Client.Core;

public static class AppInfo
{
    // Manual release label shown in the About page. Keep the "(build N)" number in sync with APP_BUILD
    // in AuthifyPass.Client/wwwroot/service-worker.published.js so the release label and the live cache
    // build shown next to it always refer to the same shipped build.
    public const string Version = "1.0.0 (build 2)";
}
