using Microsoft.AspNetCore.Mvc;
namespace SportsStore.Controllers
{
    public class Errorcontroller : Controller
    {
        public ViewResult Error() => View();
    }
}
