using System.Web.Mvc;

namespace CNWEB.Areas.Index
{
    public class IndexAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Index";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Index_default",
                "Index/{controller}/{action}/{id}",
                new { action = "Index", controller = "Index", id = UrlParameter.Optional }
            );

        }
    }
}