using Microsoft.AspNetCore.Mvc;

namespace cancel.Controllers
{
    public class HostedController
    {

        public HostedController()
        {
        }

        public ActionResult<int> Index()
        {
            return 1;
        }
    }
}