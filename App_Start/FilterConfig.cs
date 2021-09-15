using System.Web;
using System.Web.Mvc;

namespace ASP_Lab_clans
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
