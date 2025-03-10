using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Admin.FoodTypes
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitofWork;

        [BindProperty]
        public FoodType FoodTypeObj { get; set; }

        public UpsertModel(IUnitOfWork unitofWork) => _unitofWork = unitofWork;


        public IActionResult OnGet(int? id)
        {
            FoodTypeObj = new FoodType();

            if (id != 0)
            {
                FoodTypeObj = _unitofWork.FoodType.Get(u => u.Id == id);

                if (FoodTypeObj == null)
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
            if (FoodTypeObj.Id == 0)
            {
                _unitofWork.FoodType.Add(FoodTypeObj);
            }
            // existing
            else
            {
                _unitofWork.FoodType.Update(FoodTypeObj);
            }

            _unitofWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
