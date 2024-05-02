using Boo.Application.Common.Interfaces;
using Boo.Domain.Entities;
using Boo.Infrastructure.Data;
using Boo.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Boo.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        /*private readonly ApplicationDbContext _db;*/
        private readonly IUnitOfWork _unitOfWork;

        /*public VillaNumberController(ApplicationDbContext db)*/
        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            /*_db = db;*/
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            /*var villaNumbers = _db.VillaNumbers.ToList();*/
            /*var villaNumbers = _db.VillaNumbers.Include(u => u.Villa).ToList();*/
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            /*            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(u => new SelectListItem
                        {
                            Text = u.Name,
                            Value = u.Id.ToString()
                        });
                        *//*ViewData["VillaList"] = list;*//*
                        ViewBag.VillaList=list;
                        return View();*/
            VillaNumberVM villaNumberVM = new()
            {
                /*VillaList = _db.Villas.ToList().Select(u => new SelectListItem*/
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(villaNumberVM);
        }
    

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            ModelState.Remove("Villa");
            Console.WriteLine(obj);
            /*bool roomNumberExists = _db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);*/
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);


            if (ModelState.IsValid && !roomNumberExists)

                if (ModelState.IsValid)
            {
                    /* _db.VillaNumbers.Add(obj.VillaNumber);
                     _db.SaveChanges();*/
                    _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                    _unitOfWork.Save();
                    TempData["success"] = "The villa Number has been created successfully.";
                return RedirectToAction("Index");
            }
            if (roomNumberExists)
            {
                TempData["error"] = "The villa Number already exists.";
            }
            /*obj.VillaList = _db.Villas.ToList().Select(u => new SelectListItem*/
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }

        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                /*VillaList = _db.Villas.ToList().Select(u => new SelectListItem*/
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                /*VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)*/
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }


        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                /* _db.VillaNumbers.Update(villaNumberVM.VillaNumber);
                 _db.SaveChanges();*/
                _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa Number  has been updated successfully.";
                return RedirectToAction("Index");
            }
            /*villaNumberVM.VillaList = _db.Villas.ToList().Select(u => new SelectListItem*/
            villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(villaNumberVM);
        }



        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                /*VillaList = _db.Villas.ToList().Select(u => new SelectListItem*/
                 VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                 {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                /*VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)*/
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }


        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            /*VillaNumber? objFromDb = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);*/
            VillaNumber? objFromDb = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (objFromDb is not null)
            {
                /*_db.VillaNumbers.Remove(objFromDb);
                _db.SaveChanges();*/
                _unitOfWork.VillaNumber.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The villa number  has been deleted successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa number  could not be deleted.";
            return View();
        }
    }
}
