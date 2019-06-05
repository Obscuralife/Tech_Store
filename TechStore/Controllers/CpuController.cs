using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TS.DataAccessLayer.Models;
using TechStore.Data;
using TechStore.Repository;

namespace TechStore.Controllers
{

    public class CpuController : Controller
    {
        private readonly CpuRepository _repository;

        public CpuController(StoreDbContext context)
        {
            _repository = new CpuRepository(context);
        }

        // GET: Cpus
        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAllProductsAsnyc());
        }

        // GET: Cpus/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cpu = await _repository._context.Cpus.FirstOrDefaultAsync(m => m.Id == id);
            if (cpu == null)
            {
                return NotFound();
            }

            return View(cpu);
        }

        // GET: Cpus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cpus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModelSeries,Socket,Cores,CoreFrequence,Cache,CategoryName,Id,Vendor,Year,Price,Description")] Cpu cpu)
        {
            if (ModelState.IsValid)
            {
                await _repository.SaveAsync(cpu);
                return RedirectToAction(nameof(Index));
            }
            return View(cpu);
        }

        // GET: Cpus/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cpu = await _repository._context.Cpus.FindAsync(id);
            if (cpu == null)
            {
                return NotFound();
            }
            return View(cpu);
        }

        // POST: Cpus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ModelSeries,Socket,Cores,CoreFrequence,Cache,CategoryName,Id,Vendor,Year,Price,Description")] Cpu cpu)
        {
            if (id != cpu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository._context.Update(cpu);
                    await _repository._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CpuExists(cpu.Id))
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
            return View(cpu);
        }

        // GET: Cpus/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cpu = await _repository.GetProductByIdAsync((long)id);

            if (cpu == null)
            {
                return NotFound();
            }

            return View(cpu);
        }



        // POST: Cpus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _repository.RemoveProductByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CpuExists(long id)
        {
            return _repository._context.Cpus.Any(e => e.Id == id);
        }
    }
}
