using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CitasBufete.Data;
using CitasBufete.Models;

namespace CitasBufete.Controllers
{
    public class CitasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Citas
        public async Task<IActionResult> Index()
        {
              return _context.Cita != null ? 
                          View(await _context.Cita.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Cita'  is null.");
        }

        // GET: Citas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cita == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // GET: Citas/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Id_cliente") != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Clientes");

        }

        // POST: Citas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha_solicitud,Id_cliente,Nombre_cliente,Especialidad,Fecha,Hora")] Cita cita)
        {
            if (ModelState.IsValid)
            {
                if (new[] { DayOfWeek.Sunday, DayOfWeek.Saturday }.Contains(cita.Fecha.DayOfWeek))
                {
                    ModelState.AddModelError(nameof(Cita.Fecha), "Solo puede agendar citas de lunes a viernes");
                    return View(cita);
                }
                if ((DateTime.Now.Date.AddDays(1)) > cita.Fecha)
                {
                    ModelState.AddModelError(nameof(Cita.Fecha), "Solo puede agendar citas desde mañana y a más tardar 22 días después");
                    return View(cita);
                }
                if (22<CountDays(cita.Fecha))
                {
                    ModelState.AddModelError(nameof(Cita.Fecha), "Solo puede agendar citas desde el día siguiente y a más tardar 22 días después");
                    return View(cita);
                }
                var citas = from c in _context.Cita select c;
                citas = citas.Where(c => c.Fecha == cita.Fecha);
                foreach (var item in citas)
                {
                    if (item.Especialidad==cita.Especialidad && item.Hora == cita.Hora)
                    {
                        ModelState.AddModelError(nameof(Cita.Hora), "No existen espacios disponibles para esa especialidad a la hora seleccionada");
                        return View(cita);
                    }

                }
                citas = from c in _context.Cita select c;
                citas = citas.Where(c => c.Id_cliente == (int)HttpContext.Session.GetInt32("Id_cliente"));
                foreach (var item in citas)
                {
                    if (item.Especialidad == cita.Especialidad && item.Fecha > DateTime.Now.Date)
                    {
                        ModelState.AddModelError(nameof(Cita.Especialidad), "Ya cuenta con una cita para la especialidad seleccionada");
                        return View(cita);
                    }

                }

                cita.Fecha_solicitud = DateTime.Now.Date;
                cita.Id_cliente= (int)HttpContext.Session.GetInt32("Id_cliente");
                cita.Nombre_cliente= HttpContext.Session.GetString("Nombre_cliente");
                _context.Add(cita);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details",cita);
            }
            return View(cita);
            
        }

        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cita == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            return View(cita);
        }

        // POST: Citas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha_solictud,Id_cliente,Nombre_cliente,Especialidad,Fecha,Hora")] Cita cita)
        {
            if (id != cita.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExists(cita.Id))
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
            return View(cita);
        }

        // GET: Citas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cita == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cita == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cita'  is null.");
            }
            var cita = await _context.Cita.FindAsync(id);
            if (cita != null)
            {
                _context.Cita.Remove(cita);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("CitasCliente","Clientes");
        }

        private bool CitaExists(int id)
        {
          return (_context.Cita?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public static int CountDays(DateTime enddate)
        {
            var diff =  enddate- DateTime.Now;
            return (int)diff.TotalDays; 
        }

         
        
    }
}
