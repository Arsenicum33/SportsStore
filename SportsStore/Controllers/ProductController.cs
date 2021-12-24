using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;
using SportsStore.Models.ViewModels;
namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }
        public ViewResult List(string category, int productPage = 1)
        {
            return View(new ProductsListViewModel
            {
                Products = repository.Products
                                      .OrderBy(p => p.ProductID)
                                      .Where(p => category == null || p.Category == category)
                                      .Skip((productPage - 1) * PageSize)
                                      .Take(PageSize),
                Paginginfo = new Paginginfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? repository.Products.Count()
                     : repository.Products.Where(x => x.Category == category).Count()
                },
                CurrentCategory = category

            });
        }
    }
}


