using EntityFrameWork.Data;
using EntityFrameWork.Models;
using EntityFrameWork.Services.Interfaces;
using EntityFrameWork.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EntityFrameWork.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IBasketService _basketService;
        private readonly IProductService _productService;
        public HomeController(AppDbContext context, IProductService productService, IBasketService basketService)
        {
            _context = context;
            _basketService = basketService;
            _productService=productService;
        }

        public async Task<IActionResult> Index()
        {

            HttpContext.Session.SetString("name", "Pervin"); 
            Response.Cookies.Append("surname", "Rehimli" , new CookieOptions { MaxAge = TimeSpan.FromMinutes(30)});

           
         

            //Response.Cookies.Append("book",JsonConvert.SerializeObject(book));

            List<Slider> sliders = _context.Sliders.ToList();
            SliderInfo sliderInfo = _context.SliderInfos.FirstOrDefault();
            IEnumerable<Blog> blogs = _context.Blogs.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Category>categories=_context.Categories.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Product> products = await _productService.GetAll();
            IEnumerable<About>abouts= _context.Abouts.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Advantage> advantages = _context.Advantages.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Flower> flowers = _context.Flowers.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Workers> workers = _context.Workers.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Florist> florists = _context.Florists.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Instagram> instagrams = _context.Instagrams.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Subscribe> subscribes = _context.Subscribes.Where(m => !m.SoftDelete).ToList();
            HomeVm model = new()
            {
                Sliders = sliders,
                Sliderİnfo = sliderInfo,
                Blogs=blogs,
                Categories=categories,
                Products=products,
                Abouts=abouts,
                Advantages=advantages,
                Flowers=flowers,
                Workers=workers,
                Florists = florists,
                Instagrams=instagrams,
                Subscribes=subscribes
                
            };



            return View(model);
        }

        [HttpPost]
        
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id is  null) return BadRequest();

            Product? dbProduct = await _productService.GetById((int)id);

            if (dbProduct == null) return NotFound();

            List<BasketVM> basket = _basketService.GetBasketDatas();


            BasketVM? existProduct=basket?.FirstOrDefault(m=>m.Id==dbProduct.Id);

            _basketService.SetDatasToBasket (existProduct,dbProduct,basket);

            //List<BasketVM>basketDatas=GetBasketDatas();

            int basketCount = basket.Sum(m => m.Count);

            return Ok(basketCount);
        }
    }
   
    }


