namespace AuthifyPass.API.Core.Services;
internal class StringCulture<TModel>(IHttpContextAccessor contextAccessor, IStringLocalizer<TModel> localizer) : IStringCulture<TModel>
{
    public string this[string key]
    {
        get
        {
            using ThreadCultureScope cultureScope = new ThreadCultureScope(GetCurrentLanguage());
            return localizer[key];
        }
    }

    public string GetCurrentLanguage()
    {
        var httpContext = contextAccessor.HttpContext;
        string result = "en";
        if (httpContext is not null)
        {
            string acceptLanguage = httpContext.Request.Headers["Accept-Language"].ToString();
            string language = acceptLanguage.Split(',').FirstOrDefault() ?? "en";
            result = language;
        }
        return result;
    }

    public string GetString(string key)
    {
        using ThreadCultureScope cultureScope = new ThreadCultureScope(GetCurrentLanguage());
        return localizer.GetString(key);
    }
}
