using AllUpFTB.Extensions;
using AllUpFTB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace AllUpFTB.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        public readonly AppDbContext _context;
        public readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Product> products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            ViewBag.Brends = _context.Brends.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            ViewBag.Brends = _context.Brends.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (!ModelState.IsValid) return View();
            if (!_context.Brends.Any(x => x.Id == product.BrendId))
            {
                ModelState.AddModelError("BrendId", "Brend not found!");
                return View();
            }

            bool check = true;
            if (product.TagIds != null)
            {
                foreach (var tagId in product.TagIds)
                {
                    if (_context.Tags.Any(x => x.Id == tagId))
                    {
                        check = false;
                        break;
                    }
                }
            }
            if (!check)
            {
                foreach (var tagId in product.TagIds)
                {
                    ProductTag productTag = new ProductTag()
                    {
                        Product = product,
                        TagId = tagId
                    };
                    _context.ProductTags.Add(productTag);
                }
            }
            else
            {
                ModelState.AddModelError("TagId", "Tag not faund!");
                return View();
            }
            if (product.ProductPosteImageFile != null)
            {
                if (product.ProductPosteImageFile.ContentType != "image/png" && product.ProductPosteImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ProductPosteImageFile", "can only upload .png or .jpeg");
                    return View();
                }

                if (product.ProductPosteImageFile.Length > 1048576)
                {
                    ModelState.AddModelError("ProductPosteImageFile", "file size must be lower than");
                    return View();
                }
                ProductImage productImage = new ProductImage
                {
                    Product = product,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "upload/products", product.ProductPosteImageFile),
                    IsPoster = true
                };
                _context.ProductImages.Add(productImage);
            }
            if (product.ProductHoverImageFile != null)
            {
                if (product.ProductHoverImageFile.ContentType != "image/png" && product.ProductHoverImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("BookHoverImageFile", "can only upload .png or .jpeg");
                    return View();
                }

                if (product.ProductHoverImageFile.Length > 1048576)
                {
                    ModelState.AddModelError("BookHoverImageFile", "file size must be lower than");
                    return View();
                }
                ProductImage productImage = new ProductImage()
                {
                    Product = product,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "upload/products", product.ProductHoverImageFile),
                    IsPoster = false
                };
                _context.ProductImages.Add(productImage);
            }
            if (product.ImageFile != null)
            {
                foreach (var imageFile in product.ImageFile)
                {
                    if (imageFile.ContentType != "image/png" && imageFile.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("ImageFile", "can only upload .png or .jpeg");
                        return View();
                    }

                    if (imageFile.Length > 1048576)
                    {
                        ModelState.AddModelError("ImageFile", "file size must be lower than");
                        return View();
                    }
                    ProductImage bookImage = new ProductImage
                    {
                        Product = product,
                        ImageUrl = Helper.SaveFile(_env.WebRootPath, "upload/products", imageFile),
                        IsPoster = null
                    };
                    _context.ProductImages.Add(bookImage);
                }
            }
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("index");
        }


        public IActionResult Update(int id)
        {

            ViewBag.Brends = _context.Brends.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            Product existProduct = _context.Products.Include(x => x.ProductTags).Include(x=>x.ProductImages).FirstOrDefault(x => x.Id == id);
            if (existProduct == null)
            {
                return NotFound();
            };
            existProduct.TagIds = existProduct.ProductTags.Select(bt => bt.TagId).ToList();

            return View(existProduct);
        }
        [HttpPost]
        public IActionResult Update(Product product)
        {
            ViewBag.Brends = _context.Brends.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (!ModelState.IsValid) return View();
            if (!_context.Brends.Any(g => g.Id == product.BrendId))
            {
                ModelState.AddModelError("BrandId", "Brand not found!");
                return View();
            }

            Product existProduct = _context.Products.Include(b=>b.ProductImages).FirstOrDefault(b => b.Id == product.Id);
            if (existProduct == null) return NotFound();


            var existproduct = _context.Products.Include(x => x.ProductTags).FirstOrDefault(x => x.Id == product.Id);
            if (existproduct == null)
            {
                return NotFound();
            }


            existProduct.ProductTags.RemoveAll(bt => !product.TagIds.Contains(bt.TagId));

            foreach (var tagId in product.TagIds.Where(t => !existProduct.ProductTags.Any(bt => bt.TagId == t)))
            {
                ProductTag productTag = new ProductTag
                {
                    TagId = tagId
                };
                existProduct.ProductTags.Add(productTag);
            }

            if (product.ProductPosteImageFile != null)
            {
                if (product.ProductPosteImageFile.ContentType != "image/png" && product.ProductPosteImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ProductPosteImageFile", "can only upload .png or .jpeg");
                    return View();
                }

                if (product.ProductPosteImageFile.Length > 1048576)
                {
                    ModelState.AddModelError("ProductPosteImageFile", "file size must be lower than");
                    return View();
                }
                ProductImage productImage = new ProductImage
                {
                    Product = product,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "upload/products", product.ProductPosteImageFile),
                    IsPoster = true
                };
                existProduct.ProductImages.Add(productImage);
            }
            if (product.ProductHoverImageFile != null)
            {
                if (product.ProductHoverImageFile.ContentType != "image/png" && product.ProductHoverImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("BookHoverImageFile", "can only upload .png or .jpeg");
                    return View();
                }

                if (product.ProductHoverImageFile.Length > 1048576)
                {
                    ModelState.AddModelError("BookHoverImageFile", "file size must be lower than");
                    return View();
                }
                ProductImage productImage = new ProductImage()
                {
                    Product = product,
                    ImageUrl = Helper.SaveFile(_env.WebRootPath, "upload/products", product.ProductHoverImageFile),
                    IsPoster = false
                };
                existProduct.ProductImages.Add(productImage);
            }
            existproduct.ProductImages.RemoveAll(x => !product.ImageIds.Contains(x.Id) && x.IsPoster == null);
            if (product.ImageFile != null)
            {
                foreach (var imageFile in product.ImageFile)
                {
                    if (imageFile.ContentType != "image/png" && imageFile.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("ImageFile", "can only upload .png or .jpeg");
                        return View();
                    }

                    if (imageFile.Length > 1048576)
                    {
                        ModelState.AddModelError("ImageFile", "file size must be lower than");
                        return View();
                    }
                    ProductImage bookImage = new ProductImage
                    {
                        Product = product,
                        ImageUrl = Helper.SaveFile(_env.WebRootPath, "upload/products", imageFile),
                        IsPoster = null
                    };
                    existProduct.ProductImages.Add(bookImage);
                }
            }
            existProduct.Name = product.Name;
            existProduct.Description = product.Description;
            existProduct.SalePrice = product.SalePrice;
            existProduct.CostPrice = product.CostPrice;
            existProduct.DiscountPercent = product.DiscountPercent;
            existProduct.IsAvailable = product.IsAvailable;
            existProduct.ExTax = product.ExTax;
            existProduct.Code = product.Code;
            existProduct.BrendId = product.BrendId;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            ViewBag.Brends = _context.Brends.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            Product existProduct = _context.Products.FirstOrDefault(x => x.Id == id);
            if (existProduct == null) return NotFound();
            return View(existProduct);
        }
        [HttpPost]
        public IActionResult Delete(Product product)
        {
            ViewBag.Brends = _context.Brends.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            Product existProduct = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if (existProduct == null) return NotFound();

            _context.Products.Remove(existProduct);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
