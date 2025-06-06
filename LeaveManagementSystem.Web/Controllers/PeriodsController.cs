using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Web.Data;
using AutoMapper;
using LeaveManagementSystem.Web.Models.Periods;
using LeaveManagementSystem.Web.Services.Periods;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize(Roles = StaticRoles.Administrator)]
    public class PeriodsController(IPeriodsService _periodsService) : Controller
    {
       

        // GET: Periods
        public async Task<IActionResult> Index()
        {
            var data = await _periodsService.GetResult();
            return View(data);

        }

        // GET: Periods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var period = await _periodsService.GetT<PeriodReadOnlyVM>(id.Value);
               
            if (period == null)
            {
                return NotFound();
            }

            return View(period);
        }

        // GET: Periods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Periods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PeriodCreateVM _periodCreate)
        {
            if (ModelState.IsValid)
            {
                await _periodsService.Create(_periodCreate);
                return RedirectToAction(nameof(Index));
            }
            return View(_periodCreate);
        }

        // GET: Periods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var period = await _periodsService.GetT<PeriodEditVM>(id);
            if (period == null)
            {
                return NotFound();
            }
            return View(period);
        }

        // POST: Periods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PeriodEditVM  periodEdit)
        {
            if (id != periodEdit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _periodsService.Edit(periodEdit);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeriodExists(periodEdit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(periodEdit);
        }

        // GET: Periods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var period = await _periodsService.GetT<PeriodReadOnlyVM>(id.Value);

            if (period == null)
            {
                return NotFound();
            }

            return View(period);
        }

        // POST: Periods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _periodsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool PeriodExists(int id)
        {
            return _periodsService.PeriodExists(id);
        }
    }
}
