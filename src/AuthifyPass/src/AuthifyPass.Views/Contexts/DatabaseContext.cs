namespace AuthifyPass.Views.Contexts;
internal class DatabaseContext(IJSRuntime js) : StoreContext<DatabaseContext>(js)
{
    public StoreSet<TwoFactorCodeModel> Codes { get; set; }
}
