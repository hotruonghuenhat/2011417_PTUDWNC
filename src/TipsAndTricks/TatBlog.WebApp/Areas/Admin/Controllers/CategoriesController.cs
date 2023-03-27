using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Areas.Admin.Controllers {
    public class CategoriesController : Controller {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryEditModel> _validator;

        public CategoriesController(IBlogRepository blogRepository, IMapper mapper) {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _validator = new CategoryValidator(_blogRepository);
        }

        public async Task<IActionResult> Index(CategoryFilterModel model,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10) {
            var query = _mapper.Map<CategoryQuery>(model);

            var categories = await _blogRepository.GetCategoriesByQuery(query, pageNumber, pageSize);
            ViewBag.Categories = categories;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var category = id > 0
                ? await _blogRepository.FindCategoryByIdAsync(id)
                : null;

            var model = category == null
                ? new CategoryEditModel()
                : _mapper.Map<CategoryEditModel>(category);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditModel model) {
            var isValidation = await _validator.ValidateAsync(model);

            if (!isValidation.IsValid) {
                isValidation.AddToModelState(ModelState);
            }

            if (!ModelState.IsValid) {
                return View(model);
            }

            var category = model.Id > 0
                ? await _blogRepository.FindCategoryByIdAsync(model.Id) : null;

            if (category == null) {
                category = _mapper.Map<Category>(model);
                category.Id = 0;
            }
            else {
                _mapper.Map(model, category);
            }

            await _blogRepository.AddOrEditCategoryAsync(category);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCategory(int id) {
            var post = await _blogRepository.FindCategoryByIdAsync(id);
            await _blogRepository.DeleteCategoryByIdAsync(post.Id);
            return RedirectToAction(nameof(Index));
        }

    }
}
