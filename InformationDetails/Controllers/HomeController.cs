using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using InformationDetails.Models;
using System.Net;
using System.Web.Security;

namespace InformationDetails.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        InformationDetailsEntities informationDetailsEntities = new InformationDetailsEntities();
        List<CustomerDetali> customerDetalis;


        
        [HttpGet]
        public ActionResult GetDetails(string searchBy, string search)
        {
          
            if (searchBy== "CustomerName")
            {
               customerDetalis = informationDetailsEntities.CustomerDetalis.Where(x => x.CustomerName.StartsWith(search)|| search == null).ToList();
            }
            else if(searchBy== "Phone")
            {
               customerDetalis = informationDetailsEntities.CustomerDetalis.Where(x => x.Phone.StartsWith(search)|| search == null).ToList();
            }
            else 
            {
                 customerDetalis = informationDetailsEntities.CustomerDetalis.Where(x => x.City.StartsWith(search)|| search == null).ToList();
            }
            return View(customerDetalis);
        }

        [HttpPost]
        public ActionResult GetDetails()
        {
              List<CustomerDetali> customers = informationDetailsEntities.CustomerDetalis.ToList();
            if(customers.Count==0)
            {
                return View();
            }
            return View(customers);
            
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerDetali customer)
        {
            if (ModelState.IsValid)
            {
                CustomerDetali detali = new CustomerDetali();
               
                bool detalinew = informationDetailsEntities.CustomerDetalis.Any(x => x.CustomerName == customer.CustomerName);
                   if (detalinew)
                   {
                    //ViewBag.CustomerName = "CustomerName is allrady in database";
                    ModelState.AddModelError("CustomerName", "CustomerName is allrady in database");
                   
                   }

                else
                {
                    detali.CustomerName = customer.CustomerName;
                    detali.Email = customer.Email;
                    detali.Phone = customer.Phone;
                    detali.CustomerAddress = customer.CustomerAddress;
                    detali.City = customer.City;
                    detali.images = customer.images;
                    informationDetailsEntities.CustomerDetalis.Add(detali);
                    informationDetailsEntities.SaveChanges();
                    return RedirectToAction("GetDetails");
                }
            }
            return View();

        }


        //Remotly validation check username already exists in database.
        //public JsonResult IsUserNameAvailable(CustomerDetali customer)
        //{
        //   // return Json(!informationDetailsEntities.CustomerDetalis.Any(u => u.CustomerName == customer.CustomerName), JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public ActionResult Edit( int ? id)
        {
            //CustomerDetali customer = new CustomerDetali();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerDetali customerDetali = informationDetailsEntities.CustomerDetalis.Find(id);
            if (customerDetali == null)
            {
                return HttpNotFound();
            }
            return View(customerDetali);
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,CustomerName,Email,City,CustomerAddress,Phone,images")]CustomerDetali customer)
        {
            if (ModelState.IsValid)
            {

                CustomerDetali updatedCustomer = (from c in informationDetailsEntities.CustomerDetalis
                                                  where c.Id == customer.Id
                                                  select c).FirstOrDefault();
                updatedCustomer.CustomerName = customer.CustomerName;
                updatedCustomer.Email = customer.Email;
                updatedCustomer.City = customer.City;
                updatedCustomer.CustomerAddress = customer.CustomerAddress;
                updatedCustomer.Phone = customer.Phone;
                updatedCustomer.images = updatedCustomer.images;
                //informationDetailsEntities.SaveChanges();
                
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int ? id)
        {
            CustomerDetali detali = new CustomerDetali();
             detali = informationDetailsEntities.CustomerDetalis.Where(val => val.Id == id).AsEnumerable().Select(val => new CustomerDetali()
            {
                Id = val.Id,
                CustomerName = val.CustomerName,
                CustomerAddress = val.CustomerAddress,
                Email = val.Email,
                Phone=val.Phone,
                City=val.City,
                images=val.images
            }).SingleOrDefault();
            return View(detali);
        }

        [HttpPost]
        public ActionResult Delete(CustomerDetali customer)
        {
            CustomerDetali emp = informationDetailsEntities.CustomerDetalis.Where(val => val.Id == customer.Id).Single<CustomerDetali>();
            informationDetailsEntities.CustomerDetalis.Remove(emp);
            informationDetailsEntities.SaveChanges();
            return RedirectToAction("GetDetails");

        }

        public ActionResult Details(int? id)
        {
            CustomerDetali detali = new CustomerDetali();
            detali = informationDetailsEntities.CustomerDetalis.Where(val => val.Id == id).ToList().Select(val => new CustomerDetali()
            {
                Id = val.Id,
                CustomerName = val.CustomerName,
                CustomerAddress = val.CustomerAddress,
                Email = val.Email,
                Phone = val.Phone,
                City = val.City,
                images = val.images
            }).SingleOrDefault();
            return View(detali);
        }
    }
}