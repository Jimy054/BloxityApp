using BloxityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace BloxityApp.Controllers
{
    public class ProveedorController : Controller
    {
        // GET: ProductoController
        public ActionResult Index()
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
                        ViewBag.quantity = proveedores.Count();
                    }
                    else //web api sent error response 
                    {
                        proveedores = Enumerable.Empty<ProveedorModel>();
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
                catch (Exception)
                {
                    ViewBag.Message = "Error connecting!";
                }               
            }
            return View(proveedores);
        }

        [Authorize]
        public ActionResult IndexProveedoresEliminados()
        {
            IEnumerable<ProveedorModel> proveedores = null;

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(GlobalUrl.Url);
                    //HTTP GET
                    var responseTask = client.GetAsync("Proveedores/ProveedoresEliminados");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadFromJsonAsync<IList<ProveedorModel>>();
                        readTask.Wait();
                        proveedores = readTask.Result;
                        ViewBag.quantity = proveedores.Count();
                    }
                    else //web api sent error response 
                    {
                        proveedores = Enumerable.Empty<ProveedorModel>();
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
                catch (Exception)
                {
                    ViewBag.Message = "Error connecting!";
                }
            }
            return View(proveedores);
        }

        // GET: ProductoController/Details/5
        public ActionResult Detail(int id)
        {
            ProveedorModel proveedor = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalUrl.Url+"Proveedores/");
                //HTTP GET
                var responseTask = client.GetAsync("" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadFromJsonAsync<ProveedorModel>();
                    readTask.Wait();
                    proveedor = readTask.Result;
                }
            }
            return View(proveedor);
        }


        public ProveedorModel DetallesProveedor(int id)
        {
            ProveedorModel proveedor = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalUrl.Url + "Proveedores/");
                //HTTP GET
                var responseTask = client.GetAsync("" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadFromJsonAsync<ProveedorModel>();
                    readTask.Wait();
                    proveedor = readTask.Result;
                }
            }
            return proveedor;
        }

        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(ProveedorModel proveedor)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(GlobalUrl.Url +"Proveedores");
                    //HTTP POST
                    proveedor.FechaDeCreacion = DateTime.Now;
                    proveedor.Estado = "Creado";
                    var postTask = client.PostAsJsonAsync<ProveedorModel>("", proveedor);
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
            return View(proveedor);
        }





        // POST: ProductoController/Create


        // GET: ProductoController/Edit/5
        public ActionResult Edit(int id)
        {
            ProveedorModel proveedor = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalUrl.Url+"Proveedores/");
                //HTTP GET
                var responseTask = client.GetAsync("" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadFromJsonAsync<ProveedorModel>();
                    readTask.Wait();

                    proveedor = readTask.Result;
                }
            }
            return View(proveedor);
        }

        [HttpPost]
        public ActionResult Edit(long id, ProveedorModel proveedor)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalUrl.Url+"Proveedores/EditarProveedor/"+id);
                //HTTP POST
                proveedor.ProveedorId=id;
                proveedor.FechaDeModificacion = DateTime.Now;
                var putTask = client.PostAsJsonAsync<ProveedorModel>("", proveedor);
                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return View(proveedor);
        }


        public ActionResult Delete(int id)
        {
            ProveedorModel proveedor = null;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalUrl.Url + "Proveedores/Contiene/" + id);
                var putTask = client.PostAsJsonAsync<ProveedorModel>("", proveedor);
                putTask.Wait();

                var result = putTask.Result;
                var readTask = result.Content.ReadFromJsonAsync<bool>();
                if (readTask.Result)
                {

                    proveedor = DetallesProveedor(id);
                    return View(proveedor);
                }
                else
                {
                    proveedor = DetallesProveedor(id);
                    BorrarSoloProveedor(id, proveedor);
                }
            }
            return RedirectToAction("Index");
        }
        // Post: ProductoController/Delete


        [HttpPost]
        public ActionResult Delete(long id,ProveedorModel proveedor)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalUrl.Url + "Proveedores/");
                //HTTP GET
                var responseTask = client.GetAsync("" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadFromJsonAsync<ProveedorModel>();
                    readTask.Wait();
                    proveedor = readTask.Result;

                    return View(proveedor);
                }

            }
            return View(proveedor);
        }

     /*   public ActionResult Delete(long id, ProveedorModel proveedor)
        {

            using (var client = new HttpClient())
            {
                ViewBag.Contiene = "";
                client.BaseAddress = new Uri(GlobalUrl.Url+"Proveedores/Contiene/" + id);
                var putTask = client.PostAsJsonAsync<ProveedorModel>("", proveedor);
                putTask.Wait();

                var result = putTask.Result;
                var readTask = result.Content.ReadFromJsonAsync<bool>();
                if (readTask.Result)
                {
                    ViewBag.Result = "si";
                    BorrarProveedorYProductos(id);
                    //return RedirectToAction("Index");`
                }
                else {
                    BorrarSoloProveedor(id,proveedor);
                }
            }
            return View(proveedor);
        }*/

        [HttpPost]
        public ActionResult BorrarProveedorYProductos(long ProveedorId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalUrl.Url+"Proveedores/BorrarProveedores/" + ProveedorId);              
                var putTask = client.PostAsJsonAsync<long>("", ProveedorId);
                putTask.Wait();
                var result = putTask.Result;
                result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");

        }




        [HttpPost]
        public ActionResult BorrarSoloProveedor(long id, ProveedorModel proveedor)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalUrl.Url+"Proveedores/BorrarProveedor/" +id);
                proveedor.ProveedorId = id;
                proveedor.Codigo = "";
                proveedor.RazonSocial = "";
                proveedor.Rfc = "";
                proveedor.FechaDeEliminacion = DateTime.Now;
                proveedor.Estado = "Eliminado";
                var putTask = client.PostAsJsonAsync<ProveedorModel>("", proveedor);
                putTask.Wait();
                var result = putTask.Result;
                result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(proveedor);

        }

        
    }
}
