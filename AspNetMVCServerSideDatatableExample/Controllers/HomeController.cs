using AspNetMVCServerSideDatatableExample.EntityFramework;
using AspNetMVCServerSideDatatableExample.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspNetMVCServerSideDatatableExample.Controllers
{
    public class HomeController : Controller
    {
        Repository<Users> usersRepo = new Repository<Users>();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadTable()
        {
            JsonResult result = new JsonResult();
            try
            {
                //Özelleştirilebilir verileri çekiyoruz.   
                string search = Request.Form.GetValues("search[value]")[0]; //Arama kutusuna girilen string değer.
                string draw = Request.Form.GetValues("draw")[0]; //Tabloyu yeniden çizer (oluşturur).
                string order = Request.Form.GetValues("order[0][column]")[0]; //Sıralamanın uygulanması gerek sütun.
                string orderDir = Request.Form.GetValues("order[0][dir]")[0]; //Sırasıyla artan veya azalan sıralamayı belirtmek için asc veya desc olacaktır.
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]); //Kaçıncı veriden itibaren başlayacağını belirtir.
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]); //Sayfalama sayısını alıyoruz. 

                //Listeyi tanımlıyoruz.
                IEnumerable<Users> data = null;

                int totalRecords, activeRecords, passiveRecords, recFilter;

                //Toplam kayıt sayısını tutuyoruz.
                totalRecords = usersRepo.Count();
                //Aktif kayıt sayısını tutuyoruz.
                activeRecords = usersRepo.Count(x => x.is_active);
                //Pasif kayıt sayısını tutuyoruz.
                passiveRecords = usersRepo.Count(x => x.is_active == false);

                //Aranan kelimeye boş değilse
                if (!string.IsNullOrEmpty(search) || !string.IsNullOrWhiteSpace(search))
                {
                    //Aranmak istenen kelimeyi tüm kolonlarda arıyor.
                    data = usersRepo.List().Where(x => x.first_name.ToLower().Contains(search.ToLower()) || x.last_name.ToLower().Contains(search.ToLower())
                    || x.email.ToLower().Contains(search.ToLower()) || x.gender.ToLower().Contains(search.ToLower()) ||
                    x.ip_address.ToLower().Contains(search.ToLower()) ||
                    x.date_time.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Contains(string.Format("{0: dd/MM/yyyy}", search)) ||
                    (search.ToLower().Contains("aktif") ? x.is_active == true : search.ToLower().Contains("pasif") ? x.is_active == false : x.is_active == null));
                    
                    recFilter = data.Count(); // Filtrelenen kayıt sayısını tutuyoruz.
                    data = data.Skip(startRec).Take(pageSize); // Son olarak sayfalama işlemi yapıyoruz.
                }
                else
                {
                    //Tüm kayıtlar arasında sayfalama işlemini yapıyoruz.
                    data = usersRepo.List(asNoTracking: true).Skip(startRec).Take(pageSize);
                    recFilter = totalRecords; // Toplam kayıt sayısını değişkene atıyoruz.
                }

                //Sıralama işlemini yapıyoruz
                if (!(string.IsNullOrEmpty(order) && string.IsNullOrEmpty(orderDir)))
                {
                    data = SortTableData(order, orderDir, data);
                }

                //Elde ettiğimiz verileri yeni bir liste içine atıyoruz.
                var allData = data.Select(users =>
                new
                {
                    id = users.id,
                    Name = users.first_name,
                    LastName = users.last_name,
                    Email = users.email,
                    Gender = users.gender,
                    IpAddress = users.ip_address,
                    Date = users.date_time,
                    IsActive = users.is_active
                }).ToList();
                //Verileri json formatta döndürüyoruz.
                result = this.Json(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = allData,
                    activeRecords = activeRecords,
                    passiveRecords = passiveRecords
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                result = this.Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

        private IEnumerable<Users> SortTableData(string order, string orderDir, IEnumerable<Users> data)
        {
            IEnumerable<Users> lst = null;
            try
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.first_name).ToList()
                                                                                                 : data.OrderBy(p => p.first_name).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.last_name).ToList()
                                                                                                 : data.OrderBy(p => p.last_name).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.email).ToList()
                                                                                                 : data.OrderBy(p => p.email).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.gender).ToList()
                                                                                                 : data.OrderBy(p => p.gender).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ip_address).ToList()
                                                                                                   : data.OrderBy(p => p.ip_address).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.date_time).ToList()
                                                                                                   : data.OrderBy(p => p.date_time).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.is_active).ToList()
                                                                                                 : data.OrderBy(p => p.is_active).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return lst;
        }

        public JsonResult ActiveControl(int id)
        {
            var model = usersRepo.Find(x => x.id == id);

            if (model.is_active)
            {
                model.is_active = false;
            }
            else
            {
                model.is_active = true;
            }

            usersRepo.Update(model);

            return Json(new { IsActive = model.is_active }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            Users users = usersRepo.Find(x => x.id == id);

            if (users == null)
            {
                return Json(new { IsActive = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                usersRepo.Delete(users);
                return Json(new { IsActive = true }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}