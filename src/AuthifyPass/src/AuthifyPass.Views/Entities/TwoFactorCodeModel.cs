namespace AuthifyPass.Views.Entities;

public class TwoFactorCodeModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
    public string SharedKey { get; set; }
    public DateTime CreatedAt { get; set; }
}
