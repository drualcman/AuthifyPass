namespace AuthifyPass.API.Core.Options;
public class DataBaseOptions
{
    public const string SectionKey = "ConnectionStrings";

    public string Writable { get; set; }
    public string Readable { get; set; }
    public string Totp { get; set; }
}
