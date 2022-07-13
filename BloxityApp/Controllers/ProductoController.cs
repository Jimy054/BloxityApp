using BloxityApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloxityApp.Controllers
{
    public class ProductoController : Controller
    {
        // GET: ProductoController
        public ActionResult Index()
        {
            IEnumerable<ProductoModel> producto = null;

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(GlobalUrl.Url);
                    //HTTP GET
                    var responseTask = client.GetAsync("Productos");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadFromJsonAsync<IList<ProductoModel>>();
                        readTask.Wait();
                        producto = readTask.Result;
                        ViewBag.quantity = producto.Count();
                    }
                    else
                    {
                        //Error de la api
                        producto = Enumerable.Empty<ProductoModel>();
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
                catch (Exception)
                {
                    ViewBag.Message = "Error connecting!";
                }                
            }
            return View(producto);
        }

       

        // GET: ProductoController/Create
        public ActionResult Create()
        {
            FillUnidad();
            FillProveedores();
            return View();
        }


        [HttpPost]
        public ActionResult Create(ProductoModel producto)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(GlobalUrl.Url+"Productos");

                    //HTTP POST
                    producto.FechaDeCreacion = DateTime.Now;
                    producto.Estado = "Creado";
                    var postTask = client.PostAsJsonAsync<ProductoModel>("", producto);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception)
                {
                    ViewBag.Message = "Error connecting!";
                }               
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(producto);
        }


        #region Metodos
        
        public void FillProveedores()
        {
            try
            {
                var unit1 = (from proveedor in Proveedores()
                             select new SelectListItem()
                             {
                                 Text = proveedor.RazonSocial,
                                 Value = proveedor.ProveedorId.ToString()

                             }
                   ).ToList();

                unit1.Insert(0, new SelectListItem()
                {
                    Text = "-----Selecciona un Proveedor----",
                    Value = String.Empty
                });

                ViewBag.proveedores = unit1;
            }
            catch
            {

            }
        }

        public void FillUnidad()
        {
            List<SelectListItem> unit = new()
            {
               new SelectListItem { Value = "mts", Text = "mts" },
                new SelectListItem { Value = "it ", Text = "it" },
                new SelectListItem { Value = "cm ", Text = "cm" },
                new SelectListItem { Value = "pza", Text = "pza" },
            };
            ViewBag.medida = unit;
        }

        public IEnumerable<ProveedorModel> Proveedores()
        {
            IEnumerable<ProveedorModel> proveedores = null;

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(GlobalUrl.Url);
                    //HTTP GET
                    var responseTask = client.GetAsync("Proveedores");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadFromJsonAsync<IList<ProveedorModel>>();
                        readTask.Wait();
                        proveedores = readTask.Result;
                    }
                }
                catch (Exception)
                {

                }

            }
            return proveedores;
        }
        #endregion

    }
}
