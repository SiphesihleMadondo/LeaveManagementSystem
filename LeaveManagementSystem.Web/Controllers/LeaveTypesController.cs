using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using AutoMapper;
using LeaveManagementSystem.Web.Services;

namespace LeaveManagementSystem.Web.Controllers;

public class LeaveTypesController(ILeaveTypesService _leaveTypes) : Controller
{
    private const string NameExistsValidation = "This leave type already exists in the database";

    // GET: LeaveTypes
    public async Task<IActionResult> Index()
    {
        var viewData = await _leaveTypes.GetAll();
        return View(viewData);
    }

    // GET: LeaveTypes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var leaveType = await _leaveTypes.Get<LeaveTypeReadOnlyVM>(id.Value);
        if (leaveType == null)
        {
            return NotFound();
        }
        return View(leaveType);
    }

    // GET: LeaveTypes/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: LeaveTypes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveTypeCreateVM leaveCreate)
    {

        //adding custom validation and model state error
        //check if leave type name exist
        if (await _leaveTypes.CheckIfLeaveTypeExistAsync(leaveCreate.Name))
        {
            ModelState.AddModelError(nameof(leaveCreate.Name), NameExistsValidation);
        }

        if (ModelState.IsValid)
        {
            await _leaveTypes.Create(leaveCreate);
            return RedirectToAction(nameof(Index));
        }
        return View(leaveCreate);
    }

    // GET: LeaveTypes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var leaveType = await _leaveTypes.Get<LeaveTypeEditVM>(id.Value);
        if (leaveType == null)
        {
            return NotFound();
        }

        return View(leaveType);
    }

    // POST: LeaveTypes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveTypeEdit)
    {
        if (id != leaveTypeEdit.Id)
        {
            return NotFound();
        }
        //check if leave type name exist
        if (await _leaveTypes.CheckIfLeaveTypeExistForEditAsync(leaveTypeEdit))
        {
            ModelState.AddModelError(nameof(leaveTypeEdit.Name), NameExistsValidation);
        } 

        if (ModelState.IsValid)
        {
            try
            {
                var leaveType = _leaveTypes.Edit(leaveTypeEdit);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _leaveTypes.LeaveTypeExists(leaveTypeEdit.Id))
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
        return View(leaveTypeEdit);
    }

    // GET: LeaveTypes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {

        if (id == null)
        {
            return NotFound();
        }
        var leaveType = await _leaveTypes.Get<LeaveTypeReadOnlyVM>(id.Value);
        if (leaveType == null)
        {
            return NotFound();
        }

        return View(leaveType);
    }

    // POST: LeaveTypes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _leaveTypes.Remove(id);
        return RedirectToAction(nameof(Index));
    }

}
