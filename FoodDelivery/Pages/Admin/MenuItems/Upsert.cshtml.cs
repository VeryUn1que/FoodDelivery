using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodDelivery.Pages.Admin.MenuItems
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public MenuItem MenuItem { get; set; }

        // Creating these for the dropdown
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> FoodTypeList { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = hostEnvironment;  // helps find server path
        }

        public void OnGet(int? id)
        {
            if (id != null)     // Edit mode and I also track changes (true)
            {
                MenuItem = _unitOfWork.MenuItem.Get(u => u.Id == id, true);

                var categories = _unitOfWork.Category.List();
                var foodTypes = _unitOfWork.FoodType.List();

                CategoryList = categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
                FoodTypeList = foodTypes.Select(f => new SelectListItem { Text = f.Name, Value = f.Id.ToString() });
            }

            if (MenuItem == null)   // new upsert    
            {
                MenuItem = new();   // instantiate a new instance
            }
        }

        public IActionResult OnPost(int? id)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            // if the menu item is new (create)
            if (MenuItem.Id == 0)
            {
                // was there an image submitted with the form
                if (files.Count > 0)
                {
                    // create a unique identifier for the image name
                    string fileName = Guid.NewGuid().ToString();

                    // create a variable to hold the path to the images/menuItems subfolder

                    var uploads = Path.Combine(webRootPath, @"images\menuitems\");

                    // get and preserve the extension type
                    var extension = Path.GetExtension(files[0].FileName);

                    // create the complete full path string
                    var fullPath = uploads + fileName + extension;

                    using var fileStream = System.IO.File.Create(fullPath);
                    files[0].CopyTo(fileStream);

                    // associate the image to the MenuItem Object
                    MenuItem.Image = @"\images\menuitems\" + fileName + extension;
                }

                // add the MenuItem to the database
                _unitOfWork.MenuItem.Add(MenuItem);

            }
            // the item exists, so update it
            else { 


                // get orginal menu item from DB table ( since tracking and binding is on )
                var objFromDb = _unitOfWork.MenuItem.Get(m => m.Id == MenuItem.Id, true);

                // was there an image submitted with the form
                if (files.Count > 0)
                {
                    // create a unique identifier for the image name
                    string fileName = Guid.NewGuid().ToString();

                    // create a variable to hold the path to the images/menuItems subfolder

                    var uploads = Path.Combine(webRootPath, @"images\menuitems\");

                    // get and preserve the extension type
                    var extension = Path.GetExtension(files[0].FileName);

                    // if the item in the database has an image
                    var imagePath = Path.Combine(webRootPath, objFromDb.Image.TrimStart('\\'));

                    // if the image exists, physically delete
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    // create the complete full path string
                    var fullPath = uploads + fileName + extension;

                    using var fileStream = System.IO.File.Create(fullPath);
                    files[0].CopyTo(fileStream);

                    // associate the image to the MenuItem Object
                    MenuItem.Image = @"\images\menuitems\" + fileName + extension;

                }

                else
                {
                    // add image from the existing database item to the item we're udating
                    MenuItem.Image = objFromDb.Image;
                }
            }

                // update the existing item
                _unitOfWork.MenuItem.Update(MenuItem);

            // Save changes to DB
            _unitOfWork.Commit();

            // redirect to the Menu Items page
            return RedirectToPage("./Index");
        }
    }
}
