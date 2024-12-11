namespace AuthifyPass.Client.Core.Models;
public class TwoFactorCode
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public string ClientId { get; set; }
    public string SharedKey { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CurrentCode { get; set; }
}
