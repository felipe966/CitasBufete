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
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
              return _context.Cliente != null ? 
                          View(await _context.Cliente.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Cliente'  is null.");
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre_completo,Identificacion,Medio_pago,Fecha_nacimiento")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var s_cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Identificacion == cliente.Identificacion);
                if (s_cliente != null)
                {
                    ModelState.AddModelError(nameof(Cliente.Identificacion), "La identificación ingresada ya existe");
                    return View(cliente);
                }
                if (15>=GetYearsOld(cliente.Fecha_nacimiento))
                {
                    ModelState.AddModelError(nameof(Cliente.Fecha_nacimiento), "Debe ser mayor de 15 años para registrarse");
                    return View(cliente);
                }
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                ModelState.Clear();
                return RedirectToAction("Login");
                
            }
            return View(cliente);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string identificacion, DateTime fecha_nacimiento)
        {
            if (identificacion == null || fecha_nacimiento == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Identificacion == identificacion);
            if (cliente == null || cliente.Fecha_nacimiento != fecha_nacimiento)
            {
                ModelState.AddModelError(nameof(Cliente.Fecha_nacimiento), "Los datos de autenticación no son correctos");
                return View(cliente);
            }
            HttpContext.Session.SetInt32("Id_cliente", cliente.Id);
            HttpContext.Session.SetString("Nombre_cliente", cliente.Nombre_completo);
            var citas = from c in _context.Cita select c;
            citas = citas.Where(c => c.Id_cliente == cliente.Id).OrderByDescending(c => c.Fecha);
            return View("CitasCliente", citas);
        }

        public IActionResult CitasCliente()
        {
            var citas = from c in _context.Cita select c;
            citas = citas.Where(c => c.Id_cliente == (int)HttpContext.Session.GetInt32("Id_cliente"));
            return View("CitasCliente", citas);
            return View(citas);
        }

        public IActionResult Logout()
        {
           
            return  View();
        }

        [HttpPost, ActionName("Logout")]
        public IActionResult Logout(int? id)
        {
            if (HttpContext.Session.GetString("Nombre_cliente") != null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre_completo,Identificacion,Medio_pago,Fecha_nacimiento")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cliente == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cliente'  is null.");
            }
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente != null)
            {
                _context.Cliente.Remove(cliente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
          return (_context.Cliente?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public static int GetYearsOld(DateTime birthdate)
        {
            var diff = DateTime.Now - birthdate;
            return (int)(diff.TotalDays / 365.255);
        }
    }
}
