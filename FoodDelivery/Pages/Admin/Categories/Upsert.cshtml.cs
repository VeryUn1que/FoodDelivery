using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Admin.Categories
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitofWork;

        [BindProperty]
        public Category CategoryObj { get; set; }

        public UpsertModel(IUnitOfWork unitofWork) => _unitofWork = unitofWork;


        public IActionResult OnGet(int? id)
        {
            CategoryObj = new Category();

            if (id != 0)
            {
                CategoryObj = _unitofWork.Category.Get(u => u.Id == id);

                if (CategoryObj == null)
                {
                    return NotFound();
                }
            }
            return Page(); //assume insert new mode
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); 
            }

            // If new
            if (CategoryObj.Id == 0)
            {
                _unitofWork.Category.Add(CategoryObj);
            }
            // existing
            else
            {
                _unitofWork.Category.Update(CategoryObj);
            }

            _unitofWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
