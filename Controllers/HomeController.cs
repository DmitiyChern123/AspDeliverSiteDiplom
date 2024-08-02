using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Reflection;
using WebApplication1.Entities2;

using WebApplication1.Models;

using static System.Net.Mime.MediaTypeNames;
using WebApplication1.Entities2;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private static List<Product> _pendingProducts = new List<Product>();
        private readonly ILogger<HomeController> _logger;
        private readonly DiplomContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _logFilePath = "log20240517.json";
        public HomeController(ILogger<HomeController> logger, DiplomContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }
        public ActionResult Loadfiles ()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Preview()
        {
            var file = Request.Form.Files[0];
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var jsonContent = await stream.ReadToEndAsync();
                _pendingProducts = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            }

            return Json(_pendingProducts);
        }
        [HttpPost]
        public IActionResult Confirm()
        {

            if (_pendingProducts != null && _pendingProducts.Count > 0)
            {
                //очищаем id
                foreach (var product in _pendingProducts)
                {
                    product.Id = 0; // Обнулить Id, чтобы база данных сгенерировала новый идентификатор
                    foreach (var productType in product.ProductTypes)
                    {
                        productType.Id = 0; // Обнулить Id для типов товара
                        productType.ProductId = 0; // Обнулить ProductId для типов товара
                    
                    }
                _context.Products.Add(product);
                }
                
                
                _pendingProducts.Clear();
            }
            _context.SaveChanges();
            return Ok();
        }
        public ActionResult AddProductType(EditTypesViewModel model)
        {
            if (model.productid != 0)
            {
                var productType = new ProductType
                {
                    Name = model.newtype.Name,
                    Price = model.newtype.Price,
                    ProductId = model.productid,
                    Is_delated = false
                };

                _context.ProductTypes.Add(productType);
                _context.SaveChanges();
            }
            return RedirectToAction("EditTypes", "Home", new { productid = model.productid });
            return RedirectToAction("EditTypes");
        }
        [HttpPost]
        public ActionResult UpdateProductType(int productId, int productTypeId, bool isDeleted)
        {
            var type = _context.ProductTypes.FirstOrDefault(d => d.Id == productTypeId);
            if (type != null)
            {
                type.Is_delated = isDeleted;
            }
            _context.SaveChanges();
            return RedirectToAction("EditTypes", "Home", new { productid = productId });
            // Верните соответствующий результат
            return Json(new { success = true });
        }
        private List<JObject> ReadLogEntries()
        {
            List<JObject> logEntries = new List<JObject>();
            try
            {
                using (var fileStream = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var streamReader = new StreamReader(fileStream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        var log = JObject.Parse(line);
                        logEntries.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading log file: " + ex.Message);
            }
            return logEntries;
        }
        [Authorize(Roles = "manager")]
        public ActionResult OrderEdit(int statusid)
        {
            var model = new OrdersViewModel();
            model.Orders = _context.Orders.Include(o => o.Status)
                                          .Include(o => o.User)
                                          .Include(o => o.Courier)
                                          .ToList();
            model.statuses = _context.StatusOrders.ToList();
            model.courses = _context.Couriers.Include(c => c.Orders).ToList();

            DateTime now = DateTime.Now;
            DateTime tenMinutesAgo = now.AddMinutes(-10);

            // Фильтрация курьеров, которые не имели заказов за последние 10 минут
            model.courses = model.courses
                .Where(c => c.Orders == null || !c.Orders.Any(o => o.Date >= tenMinutesAgo && o.Date <= now))
                .ToList();

            return View(model);
        }
        [Authorize(Roles = "admin")]
        public ActionResult EditTypes(int productid)

        {
            EditTypesViewModel editTypesViewModel = new EditTypesViewModel();
            editTypesViewModel.types = _context.Products.Include(d => d.ProductTypes).FirstOrDefault(d => d.Id == productid).ProductTypes.ToList();
            editTypesViewModel.productid = productid;



            return View("EditTypes", editTypesViewModel);

        }

        [Authorize(Roles = "admin")]
        public ActionResult EndEditing(EditProductViewModel model)
        {

            string s = " ";

            if (ModelState.IsValid)
            {
                var d = ModelState.ValidationState;

                string uniqueFileName = "";
                if (model.ImgFile != null && model.ImgFile.Length > 0)
                {

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImgFile.FileName;



                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);


                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImgFile.CopyToAsync(fileStream);
                    }


                }
                if (model.selcat != null)
                {

                    if (model.product.Id == 0)
                    {
                        Product product = new Product { Name = model.product.Name, Opis = model.product.Opis, Img = "images/" + uniqueFileName, Price = model.product.Price, IdCategory = model.selcat,Is_hidden=model.product.Is_hidden };
                        ProductType productType = new ProductType { Name = model.product.Name, Price = (int)model.product.Price };
                        product.ProductTypes.Add(productType);
                        _context.Products.Add(product);
                        _context.SaveChanges();

                    }
                    else if (model.product.Id > 0)
                    {

                        Product p = _context.Products.FirstOrDefault(d => d.Id == model.product.Id);

                        p.Name = model.product.Name;
                        p.Opis = model.product.Opis;
                        p.Price = model.product.Price;
                        p.Is_hidden = model.product.Is_hidden;
                        if (model.ImgFile != null)
                        {

                            p.Img = "images/" + uniqueFileName;
                        }

                        p.IdCategory = model.selcat;
                        _context.SaveChanges();
                    }
                }
                return RedirectToAction("Index", model);
            }
            else
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }
                model.categories = _context.Categories.ToList();
                return View("CreateOrEditProduct", model);
            }
        }
        public static bool AreAllPropertiesNull(object obj)
        {
            if (obj == null)
            {
                return true;
            }

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj);
                if (value != null)
                {
                    return false;
                }
            }

            return true;
        }
        public ActionResult TestOrder()
        {
            Order order = new Order
            {
                UserId = 3,
                Adress = "test value",
                Phone = "+7 900 123 4567",
                StatusId = 1,
                Date = DateTime.Now,
                Is_need_devices = true,
                PayType = "none"
                
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("OrderEdit");
        }
        public ActionResult changestatus(int orderid, int statusid)
        {
            if (orderid != 0)
            {
                var d = _context.Orders.FirstOrDefault(d => d.Id == orderid);
                d.StatusId = statusid;
                _context.SaveChanges();
            }
            return RedirectToAction("OrderEdit");
        }
        public ActionResult getCourirer(int orderid,int courierid)
        {
            if (orderid != 0)
            {

            if (courierid != 0)
            {
               var d = _context.Orders.FirstOrDefault(d=>d.Id == orderid);
                d.CourierId = courierid;
                _context.SaveChanges();
            }
            else
            {
                var d = _context.Orders.FirstOrDefault(d => d.Id == orderid);
                d.CourierId = null;
                _context.SaveChanges();
            }
            }

            return RedirectToAction("OrderEdit");
        }



        [Authorize(Roles = "admin")]
        public ActionResult CreateOrEditProduct(int productid)

        {



            EditProductViewModel editProductViewModel = new EditProductViewModel();
            Product product;
            if (productid == 0)
            {
                product = new Product();
                editProductViewModel.categories = _context.Categories.ToList();

                var fileMock = new Mock<IFormFile>();

                // Настройка свойств заглушки файла
                fileMock.Setup(f => f.FileName).Returns("test.txt");
                fileMock.Setup(f => f.Length).Returns(1024); // Размер файла в байтах
                fileMock.Setup(f => f.ContentType).Returns("text/plain");
                editProductViewModel.ImgFile = fileMock.Object;
                //product.IdCategory = 0;

            }
            else
            {
                product = _context.Products.Include(d => d.ProductTypes).FirstOrDefault(d => d.Id == productid);
                editProductViewModel.categories = _context.Categories.ToList();
                editProductViewModel.selcat = product.IdCategory;
                
                using var stream = new FileStream("wwwroot/" + product.Img, FileMode.Open);
                IFormFile formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/octet-stream"  
                };
                editProductViewModel.ImgFile = formFile;
                editProductViewModel.types = product.ProductTypes.ToList();
            }
            editProductViewModel.product = new clases.ValidateProduct(product);
            return View("CreateOrEditProduct", editProductViewModel);

        }
        public ActionResult Logout()
        {
            // Удаление аутентификационных куки
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Перенаправление на главную страницу или страницу входа
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index(int? categoryId, string sortOrder, string searchQuery)
        {
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(x => x.Name == username);
            if (user != null)
            {
                var addinp = _context.Korzinas.Include(x => x.ProductType).Where(x => x.UserId == user.Id).ToList();
                if (addinp !=null)
                {
                    ViewBag.CartInfo = addinp;

                }
            }
            else
            {
                ViewBag.CartInfo = null;
            }
            List<ProductType> productsinkorzinas = new List<ProductType>();
            if (user != null)
            {
                var d = _context.Korzinas.Include(x => x.ProductType).Where(d => d.UserId == user.Id);
                foreach (var item in d)
                {
                    productsinkorzinas.Add(item.ProductType);
                }
            }
            ViewBag.SearchQuery = searchQuery;
            var categories = _context.Categories.Include(d => d.Products).ToList();
            var products = _context.Products.Include(d => d.ProductTypes).Include(d => d.IdCategoryNavigation).AsQueryable();


            if (ViewData["CategoryList"] != null)
            {

            }
            var categoryList = _context.Categories

       .Select(c => new SelectListItem
       {
           Value = c.Id.ToString(),
           Text = c.Name
       })
       .ToList();



            
            ViewData["CategoryList"] = categoryList;
            if (categoryId != null)
            {
                if (categoryId != -1)
                {

                    products = products.Where(p => p.IdCategory == categoryId.Value);
                }
            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p => p.Name.Contains(searchQuery));
            }

            
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "Price":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            var viewModel = new HomeModel
            {
                Categories = categories,
                Products = products.ToList(),
                SelectedCategoryId = categoryId,
                SortOrder = sortOrder,
                SearchQuery = searchQuery,
                hasinkorzina = productsinkorzinas

            };

            return View(viewModel);
        }
        [Authorize(Roles = "user")]
        public IActionResult AddProduct(int productid, int propertyId)
        {

            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(x => x.Name == username);

            var item = _context.Korzinas.FirstOrDefault(d => d.ProductTypeId == productid);
            if (item == null)
            {


                Korzina korzina = new Korzina();
                korzina.ProductTypeId = propertyId;
                korzina.UserId = user.Id;
                korzina.Count = 1;

                _context.Korzinas.Add(korzina);
            }
            else
            {
                item.Count++;

            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult CheckLogin()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {

                return Json(new { isAuthenticated = false, redirectUrl = "/Accaunt/Index" });
            }
        }
        public IActionResult Products(/*string? name, int categoryId, int sortType*/)
        {

            var products = _context.Products.ToList();


            //}
            return View(new HomeModel { Products = products/*, Categories = category, SelectedCategory = categoryId, SelectedSorting = sortType*/});
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public ActionResult GetProductDetails(int productId)
        {
            // Получите дополнительную информацию о товаре по productId
            var product = _context.Products.FirstOrDefault(d => d.Id == productId);

            return PartialView("_ProductDetails", product);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> LoggerPage(DateTime? date, string errorType)
        {
            LogsViewModel logsViewModel = new LogsViewModel();

            try
            {
                List<JObject> logEntries = ReadLogEntries();

                // Фильтрация записей журнала по дате и типу ошибки
                if (date != null)
                {
                    logEntries = logEntries.Where(log => log["Timestamp"].Value<DateTime>().Date == date).ToList();
                }
                if (!string.IsNullOrEmpty(errorType))
                {
                    logEntries = logEntries.Where(log => log["Level"].Value<string>() == errorType).ToList();
                }

                logsViewModel.logs = logEntries;
            }
            catch (Exception ex)
            {
                logsViewModel.ErrorMessage = "Error reading log file: " + ex.Message;
            }

            return View(logsViewModel);
        }
    }
}
