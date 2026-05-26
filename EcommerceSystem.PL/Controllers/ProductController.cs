using EcommerceSystem.BLL;
using EcommerceSystem.PL.SystemRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.Build.Tasks;
using Microsoft.EntityFrameworkCore;



namespace EcommerceSystem.PL
{
    /*
             . Index – Display a table of all products and their categories with:
        •	link to view more details.
        •	Link to Edit.
        •	Link to Delete.
        •	Link to Add New.
        2. Details – Display detailed information for a single product based on its Id.
        3. Create Action
        Description:
        •	Display a form to create a new product.
        •	After submitting:
        •	Add the product to the DB.
        •	Redirect back to Index.
        4. Edit Action
        Description:
        •	Display a form filled with existing product data.
        •	The product should be selected using its Id.
        •	Allow updating product information.
        •	After submitting:
        •	Save changes.
        •	Redirect to Index.
        5. Delete Action
        Description:
        •	Display confirmation Modal.
        •	The product should be selected using its Id.
        •	After confirmation:
        •	Remove the product from the DB.
        •	Redirect to Index.
        */
    [Authorize]
    public class ProductController : Controller
    {
        /*---------------------------------------------------*/
        //add database
        private readonly IProductManager _productManager;

        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }
        /*---------------------------------------------------*/


        /*---------------------------------------------------*/
        // Get All Products 
        //Crud Operations
        #region GetAll Products
        [HttpGet]
        public IActionResult Index()
        {
            var Dtos = _productManager.GetAllProducts();
            if (!Dtos.Success)
            {
                //error handling
                return RedirectToAction(nameof(Index));
            }
            var VMs = Dtos?.Data?.Select(p => new ProductReadVM()
            {
                ProdId = p.ProdId,
                ProdTitle = p.ProdTitle,
                CategoryId = p.CategoryId,
                ProdDescription = p.ProdDescription,
                ProdCount = p.ProdCount,
                ProdCategory = p.ProdCategory,
                ProdPrice = p.ProdPrice,
                ExpiryDate = p.ExpiryDate,
                ImageURL = p.ImageURL,
            }).ToList();
            return View(VMs);
        }
        #endregion

        /*---------------------------------------------------*/
        //Get Details of Product
        #region Get Product BY Id
        [HttpGet]
        public IActionResult Details(int id)
        {
            var p = _productManager.GetProductById(id);
            if (!p.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            ProductReadVM vm = new ProductReadVM() {
                ProdId = p.Data.ProdId,
                ProdTitle = p.Data.ProdTitle,
                CategoryId = p.Data.CategoryId,
                ProdDescription = p.Data.ProdDescription,
                ProdCount = p.Data.ProdCount,
                ProdCategory = p.Data.ProdCategory,
                ProdPrice = p.Data.ProdPrice,
                ExpiryDate = p.Data.ExpiryDate,
                ImageURL = p.Data.ImageURL,
            };
            return View(vm);
        }
        #endregion
        //    /*---------------------------------------------------*/
        //Edit Product
        //public IActionResult Edit(int id)

        #region Edit Action
        [HttpGet]
        [Authorize(Roles = SysRoles.AdminRole)]
        public IActionResult Edit(int id)
        {
            var product = _productManager.GetProductById(id);
            if (!product.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            ProductEditVM EditProduct = new ProductEditVM()
            {
                ProdId = product.Data.ProdId,
                ProdTitle = product.Data.ProdTitle,
                ProdDescription = product.Data.ProdDescription,
                ProdPrice = product.Data.ProdPrice,
                ExpiryDate = product.Data.ExpiryDate,
                ProdCount = product.Data.ProdCount,
                ImageURL= product.Data.ImageURL,
                CategoryId = product.Data.CategoryId,
                ProdCategory = product.Data.ProdCategory,
                Categories = _productManager.GetCategoryList()?.Select(p => new CategoryReadVM()
                {
                    Id= p.Id,
                    Name = p.Name,
                }).ToList()
            };
            return View(EditProduct);
        }
        [HttpPost]
        public IActionResult Edit(ProductEditVM vm)
        {
            #region Validation Try
            ModelState.Remove("Categories");
            ModelState.Remove("ProdCategory");
            ModelState.Remove("ImageURL");
            if (!ModelState.IsValid)
            {
                vm.Categories = _productManager.GetCategoryList()?.Select(p => new CategoryReadVM()
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToList();
                return View(vm);
            }
            if (vm.Image is not null)
            {
                // Old image full path
                if(vm.ImageURL is not null)
                {
                    var oldImagePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "Images",
                    "Products",
                    vm.ImageURL);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Folder path
                var folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "Images",
                    "Products");

                // Create folder if not exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Delete old image
                

                // Generate new file name
                var fileName = Path.GetFileNameWithoutExtension(vm.Image.FileName);

                var extension = Path.GetExtension(vm.Image.FileName);

                var newFileName =
                    $"{fileName}_{Guid.NewGuid()}{extension}";

                // Full new image path
                var newFilePath = Path.Combine(
                    folderPath,
                    newFileName);

                // Save new image
                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    vm.Image.CopyTo(stream);
                }

                // Update ImageURL in the view model
                vm.ImageURL = newFileName;

            }
            #endregion
            ProductEditDTO Dto = new ProductEditDTO()
            {
                ProdId= vm.ProdId,
                ProdTitle= vm.ProdTitle,
                ProdDescription= vm.ProdDescription,
                ProdPrice= vm.ProdPrice,
                ProdCount= vm.ProdCount,
                CategoryId= vm.CategoryId,
                ExpiryDate= vm.ExpiryDate,
                ProdCategory= vm.ProdCategory,
                ImageURL= vm.ImageURL,
                
            };
            _productManager.UpdateProduct(Dto);
            return RedirectToAction(nameof(Index));

        }
        #endregion
        //    /*---------------------------------------------------*/
        #region Create Action
        [HttpGet]
        [Authorize(Roles = SysRoles.AdminRole)]
        public IActionResult Create()
        {
            ProductCreateVM vm = new ProductCreateVM()
            {
                Categories = _productManager.GetCategoryList()?.Select(p => new CategoryReadVM()
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(ProductCreateVM vm)
        {
            #region Validation layer
            ModelState.Remove("Categories");
            ModelState.Remove("ProdCategory");
            if (!ModelState.IsValid)
            {
                vm.Categories = _productManager.GetCategoryList()?.Select(p => new CategoryReadVM()
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToList();
                return View(vm); 
            }
            //create image
            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "Images",
                "Products");
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            var fileName = Path.GetFileNameWithoutExtension(vm?.Image?.FileName);
            var extension = Path.GetExtension(vm?.Image?.FileName);
            var newFileName= $"{fileName}_{Guid.NewGuid()}{extension}";

            var filePath=Path.Combine(FolderPath, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                vm?.Image?.CopyTo(stream);
            }

            // Step 2: BLL validation
            ProductCreateDTO Dto = new ProductCreateDTO()
            {
                ProdId = vm.ProdId,
                ProdTitle = vm.ProdTitle,
                ProdDescription = vm.ProdDescription,
                ProdPrice = vm.ProdPrice,
                ProdCount = vm.ProdCount,
                CategoryId = vm.CategoryId,
                ExpiryDate = vm.ExpiryDate,
                ProdCategory = vm.ProdCategory,
                ImageURL = newFileName
            };
            var result = _productManager.CreateProduct(Dto);

            
           
           
            #endregion
            return RedirectToAction(nameof(Index));
        }
        #endregion
        //    /*---------------------------------------------------*/
        #region Delete
        [HttpGet]
        [Authorize(Roles = SysRoles.AdminRole)]
        public IActionResult Delete(int id)
        {
            //? remove the image form server
            var pro= _productManager.GetProductById(id);
            if(pro?.Data?.ImageURL is not null)
            {
                var FilePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "Images",
                    "Products",
                    pro.Data.ImageURL);

                if (System.IO.File.Exists(FilePath))
                {
                    System.IO.File.Delete(FilePath);
                }

            }

            _productManager.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }


        #endregion
        //    public JsonResult IsUnique(string ProdTitle)
        //    {
        //        bool isExist = db.Products.Any(p => p.Title == ProdTitle);
        //        if (isExist)
        //        {
        //            return Json($"this Product{ProdTitle} is Already exist");
        //        }
        //        return Json(true);
        //    }
        //    /*---------------------------------------------------*/


        //    // helper function 
        //    private List<SelectListItem> GetProductSelectList()
        //    {
        //        return db.Categories.Select(c => new SelectListItem()
        //        {
        //            Value= c.Id.ToString(),
        //            Text= c.Name
        //        }).ToList();    
        //    }

    }

}
