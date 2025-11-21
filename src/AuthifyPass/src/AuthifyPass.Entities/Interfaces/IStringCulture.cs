namespace AuthifyPass.Entities.Interfaces;
public interface IStringCulture<TModel>
{
    string this[string key] { get; }
    string GetString(string key);
    string GetCurrentLanguage();
}
