using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using NuGet.Packaging;
using System.Configuration;
using Utility;

namespace GraduationProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole},{SD.Professor},{SD.Student},{SD.Asseitant},{SD.Empolyee}")]

    public class TimetableController : Controller
    {
        private readonly ITimetableRepository timetableRepository;
        private readonly ITableLecSecRepositry tableLecSecRepositry;

        public TimetableController(ITimetableRepository timetableRepository, ITableLecSecRepositry tableLecSecRepositry)
        {
            this.timetableRepository = timetableRepository;
            this.tableLecSecRepositry = tableLecSecRepositry;
        }
        public IActionResult Index()
        {
            var timetables = timetableRepository.GetAll().ToList();
            return View(timetables);
        }
        public IActionResult Details(string day)
        {
            ViewBag.day = day;
            EnumTableDay enumDay = Enum.Parse<EnumTableDay>(day);

            var timetableday = timetableRepository.GetOne([e=>e.TableLecSecs],expression: e => e.Day.Equals(enumDay)).FirstOrDefault();
            return View(model:timetableday);
        }

        public IActionResult Create(string day)
        {
            if(User.IsInRole(SD.Empolyee) || User.IsInRole(SD.AdminRole))
            {
                ViewBag.day = day;
                EnumTableDay enumDay = Enum.Parse<EnumTableDay>(day);
                var TimeDay = timetableRepository.GetOne(expression: e => e.Day.Equals(enumDay)).FirstOrDefault();
                ViewBag.DayId = TimeDay.TimetableId;

                ViewBag.EnumType = (EnumTypeTable[])Enum.GetValues(typeof(EnumTypeTable));


                return View();
            }
            return RedirectToAction("index");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TableLecSec tableLecSec)
        {
            if (User.IsInRole(SD.Empolyee) || User.IsInRole(SD.AdminRole))
            {
                if (ModelState.IsValid)
                {
                    tableLecSecRepositry.Add(tableLecSec);
                    timetableRepository.Commit();
                    return RedirectToAction("index");
                }

                ViewBag.day = TempData["day"];
                EnumTableDay enumDay = Enum.Parse<EnumTableDay>(ViewBag.day);
                var TimeDay = timetableRepository.GetOne(expression: e => e.Day.Equals(enumDay)).FirstOrDefault();
                ViewBag.DayId = TimeDay.TimetableId;

                ViewBag.EnumType = (EnumTypeTable[])Enum.GetValues(typeof(EnumTypeTable));

                return View(tableLecSec);
            }
            return RedirectToAction("index");

        }

        public IActionResult Edit(int TableLecSecId,string day)
        {
            if (User.IsInRole(SD.Empolyee) || User.IsInRole(SD.AdminRole))
            {
                ViewBag.day = day;
                TempData["day"] = day;
                ViewBag.EnumType = (EnumTypeTable[])Enum.GetValues(typeof(EnumTypeTable));

                var TableContent = tableLecSecRepositry.GetOne(expression: e => e.TableLecSecId == TableLecSecId).FirstOrDefault();
                return View(TableContent);
            }
            return RedirectToAction("index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TableLecSec tableLecSec)
        {
            if (User.IsInRole(SD.Empolyee) || User.IsInRole(SD.AdminRole))
            {
                if (ModelState.IsValid)
                {
                    tableLecSecRepositry.Edit(tableLecSec);
                    timetableRepository.Commit();
                    var id = tableLecSec.TimetableId;
                    var tableday = timetableRepository.GetOne(expression: e => e.TimetableId == id).FirstOrDefault();

                    return RedirectToAction("Details", new { day = tableday.Day });
                }


                ViewBag.EnumType = (EnumTypeTable[])Enum.GetValues(typeof(EnumTypeTable));
                ViewBag.day = TempData["day"];
                return View(tableLecSec);
            }
            return RedirectToAction("index");

        }
        public IActionResult Delete(int TableLecSecId)
        {
            if (User.IsInRole(SD.Empolyee) || User.IsInRole(SD.AdminRole))
            {
                var TableContent = tableLecSecRepositry.GetOne(expression: e => e.TableLecSecId == TableLecSecId).FirstOrDefault();
                var id = TableContent.TimetableId;
                var tableday = timetableRepository.GetOne(expression: e => e.TimetableId == id).FirstOrDefault();
                tableLecSecRepositry.Delete(TableContent);
                tableLecSecRepositry.Commit();
                return RedirectToAction("Details", new { day = tableday.Day });
            }
            return RedirectToAction("index");

        }

    }
}
