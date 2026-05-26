using EcommerceSystem.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace EcommerceSystem.PL
{
    /*
     Create a Category Controller
Create a CategoryController with the following actions:

1. Index – Display a table of all categories with:
•	link to view more details.
•	Link to Edit.
•	Link to Delete.
•	Link to Add New.
2. Details – Display detailed information for a single Category based on its Id.
3. Create Action
Description:
•	Display a form to create a new Category.
•	After submitting:
•	Add the category to the DB.
•	Redirect back to Index.
4. Edit Action
Description:
•	Display a form filled with existing category data.
•	The category should be selected using its Id.
•	Allow updating category information.
•	After submitting:
•	Save changes.
•	Redirect to Index.
5. Delete Action
Description:
•	Display confirmation Modal.
•	The category should be selected using its Id.
•	After confirmation:
•	Remove the category from the DB.
•	Redirect to Index.

     */
    [Authorize]
    public class CategoryController : Controller
    {
        // get the Categories from DB
        /*---------------------------------------------------*/
        //add database
        private ICategoryManager _categoryManager;

        /*---------------------------------------------------*/

        public CategoryController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        /*---------------------------------------------------*/
        #region Get All Categories
        [HttpGet]
        public IActionResult Index()
        {
            //get the categories from DB
            var Dtos = _categoryManager.GetAllCategories().Select(p => new CategoryReadVM()
            {
                Id = p.Id,
                Name = p.Name,
                NoProductTypes = p.NoProductTypes,
                Products= p.Products,
            }).ToList();
            return View(Dtos);
        }
        #endregion
        /*---------------------------------------------------*/
        [HttpGet]
        public IActionResult Details(int id)
        {
            var category = _categoryManager.GetCategoryById(id);
            if (category == null)
            {
                return RedirectToAction(nameof(Index));
            }
            CategoryReadVM vm = new CategoryReadVM()
            {
                Id= category.Id,
                Name = category.Name,
                Products = category.Products,
            };
            return View(vm);
        }
        /*---------------------------------------------------*/
        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            
            CategoryCreateVM CreateVm = new CategoryCreateVM();
            return View(CreateVm);
        }

        [HttpPost]
        public ActionResult Create(CategoryCreateVM CreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(CreateVM);
            }
            CategoryCreateDTO categoryCreateDTO = new CategoryCreateDTO()
            {
                Id = CreateVM.Id,
                Name= CreateVM.Name,
            };
            _categoryManager.CreateCategory(categoryCreateDTO);
            return RedirectToAction(nameof(Index));
        }
        #endregion
        /*---------------------------------------------------*/
        #region Edit Action
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _categoryManager.GetCategoryById(id);
            CategoryEditVM EditVm = new CategoryEditVM()
            {
                Id = category.Id,
                Name = category.Name
            };
            if (category == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(EditVm);
        }
        [HttpPost]
        public IActionResult Edit(CategoryEditVM cateoryEdited)
        {
            if (!ModelState.IsValid)
            {
                return View(cateoryEdited);
            }
            //map the DTO into ViewModel
            CategoryEditDTO categoryEditVM = new CategoryEditDTO()
            {
                Id= cateoryEdited.Id,
                Name= cateoryEdited.Name,
            };
            _categoryManager.UpdateCategory(categoryEditVM);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        /*---------------------------------------------------*/
        #region Delete Action
        public IActionResult Delete(int id)
        {
            _categoryManager.DeleteCategory(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
