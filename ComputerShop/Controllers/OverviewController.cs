using ComputerShop.Data;
using ComputerShop.Models;
using ComputerShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ComputerShop.Controllers
{
    public class OverviewController : Controller
    {
        private readonly ApplicationDbContext _context;
       
        public OverviewController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int dataId)
        {
            ViewData["dataId"] = dataId;
            var searchingText = HttpContext.Request.Query["searchingText"];
            OverviewPageViewModel overviewPageViewModel = new OverviewPageViewModel();
            List<Models.Product> productsToShow;        
            if (!string.IsNullOrEmpty(searchingText))
            {
                productsToShow = _context.Products.Include(x => x.Producer).Where(x => x.Name.Contains(searchingText)).ToList();
            }
            else
            {
                productsToShow = _context.Products.Include(x => x.Producer).Where(x => x.CategoryId == dataId).ToList();
            }
            foreach (var item in productsToShow)
            {
                if(item.Producer.IsPromoted == true)
                {
                    overviewPageViewModel.PromotedProducts.Add(item);                   
                } else
                {
                    overviewPageViewModel.Products.Add(item);
                }
               
            }
            var producers = _context.Products.Include(x => x.Producer).Where(x => x.CategoryId == dataId).Select(x => x.Producer).ToList();
            foreach (var item in producers)
            {
                overviewPageViewModel.Producents.Add(item.Name);
            }
            overviewPageViewModel.Producents =  overviewPageViewModel.Producents.Distinct().ToList();
            return View(overviewPageViewModel);
            
        }
        [HttpGet]
        public IActionResult FilterProducts(List<string>producer, int? minCena, int? maxCena, int dataId)
        {
            ViewData["dataId"] = dataId;
            List<Models.Product> productsToShow = _context.Products.Include(x => x.Producer).Where(x=>x.CategoryId==dataId).ToList();
            if (producer.Count > 0)
            {
                productsToShow = productsToShow.Where(x => producer.Contains(x.Producer.Name)).ToList();
            }
            if(minCena != null) {
                productsToShow = productsToShow.Where(x => x.Price >= minCena).ToList();
            
            }
            if (maxCena != null)
            {
                productsToShow = productsToShow.Where(x => x.Price <= maxCena).ToList();

            }         
            if(minCena != null && maxCena != null && minCena > maxCena) {
               productsToShow= _context.Products.Include(x => x.Producer).Where(x => x.CategoryId == dataId).ToList();
            }
            OverviewPageViewModel overviewPageViewModel = new OverviewPageViewModel();
            foreach (var item in productsToShow)
            {
                if (item.Producer.IsPromoted == true)
                {
                    overviewPageViewModel.PromotedProducts.Add(item);
                }
                else
                {
                    overviewPageViewModel.Products.Add(item);
                }
            }
            var productProducers = _context.Products.Include(x => x.Producer).Where(x => x.CategoryId == dataId).Select(x => x.Producer).ToList();
            foreach (var item in productProducers)
            {
                overviewPageViewModel.Producents.Add(item.Name);
            }
            overviewPageViewModel.Producents = overviewPageViewModel.Producents.Distinct().ToList();
            return View("Index",overviewPageViewModel);

        }
    }
}
