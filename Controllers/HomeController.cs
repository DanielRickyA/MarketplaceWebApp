using MarketWebApp.Data;
using MarketWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

namespace MarketWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Produk.ToList();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult DetailProduk(int id)
        {
            var product = _context.Produk.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult TambahKeKeranjang(int idProduk, int jumlah)
        {
            var userJson = HttpContext.Request.Cookies["User"];


            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Account");
            }

            var dbCustomer = JsonSerializer.Deserialize<Customer>(userJson);
            int idCustomer = dbCustomer.Id; 

            var produk = _context.Produk.FirstOrDefault(p => p.Id == idProduk);
            if (produk == null)
            {
                return NotFound();
            }

            var existingItem = _context.Keranjang
                .FirstOrDefault(k => k.IdProduk == idProduk && k.IdCustomer == idCustomer);

            if (existingItem != null)
            {
                existingItem.Jumlah += jumlah;
                existingItem.SubTotal = existingItem.Jumlah * (produk.Price);
                _context.Keranjang.Update(existingItem);
            }
            else
            {
                var newItem = new Keranjang
                {
                    IdProduk = idProduk,
                    IdCustomer = idCustomer,
                    Jumlah = jumlah,
                    SubTotal = jumlah * (produk.Price)
                };
                _context.Keranjang.Add(newItem);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Keranjang()
        {
            var userJson = HttpContext.Request.Cookies["User"];

            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Account");
            }

            var dbCustomer = JsonSerializer.Deserialize<Customer>(userJson);
            int idCustomer = dbCustomer.Id; 
            var keranjangItems = _context.Keranjang
                .Include(k => k.Produk) 
                .Where(k => k.IdCustomer == idCustomer) 
                .ToList();

            return View(keranjangItems);
        }

      
            [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("User");

            return RedirectToAction("Login", "Login");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
