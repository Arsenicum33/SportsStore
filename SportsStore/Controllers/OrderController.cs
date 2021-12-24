using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;
        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }

        [Authorize]
        public ViewResult List()
        {
            return View(repository.Orders.Where(о => !о.Shipped));
        }
            
        [HttpPost]
        [Authorize]
        public IActionResult MarkShipped(int orderID)
        {
            Order order = repository.Orders
            .FirstOrDefault(o => o.OrderID == orderID);
            if (order != null)
            {
                order.Shipped = true;
                repository.SaveOrder(order);
            }
            return RedirectToAction("List");
        }


        public IActionResult Checkout()
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("empty", "Sorry, your cart is empty!");
                TempData["message"] = "Sorry, your cart is empty!";
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                return View(new Order());
            }
        }
        [HttpPost]
        public IActionResult Checkout(Order order)
        {

            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }
        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
    }
}
