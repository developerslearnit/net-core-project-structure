namespace ShephardTech.Web.helpers
{
    public class CookieHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Get(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[key] ?? null;
        }

        public void remove(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

        public void Set(string key,string value, int? expirationDays)
        {
            CookieOptions option = new CookieOptions();
            if (expirationDays.HasValue)
                option.Expires = DateTime.Now.AddDays(expirationDays.Value);
            else
                option.Expires = DateTime.Now.AddDays(1);
            option.Secure = true;
            option.HttpOnly = true;
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
        }
    }
}
