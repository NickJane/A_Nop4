using System.Web.Mvc;

namespace Nop.Test
{
    public class TestAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Test";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Test_default",
                "Test/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", area = "Test", id = "" },
                new[] { "Nop.Test.Controllers" }
            );
        }
    }
}
