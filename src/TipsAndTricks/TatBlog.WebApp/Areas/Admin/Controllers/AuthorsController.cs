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
    public class AuthorsController : Controller {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<AuthorsEditModel> _validator;

        public AuthorsController(IBlogRepository blogRepository, IMapper mapper) {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _validator = new AuthorValidator(_blogRepository);
        }

        public async Task<IActionResult> Index(AuthorsFilterModel filter) {
            var query = _mapper.Map<AuthorQuery>(filter);
            var authors = await _blogRepository.GetAuthorsAsync(query);
            ViewBag.Authors = authors;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var author = id > 0
                ? await _blogRepository.FindAuthorByIdAsync(id)
                : null;

            var model = author == null
                ? new AuthorsEditModel()
                : _mapper.Map<AuthorsEditModel>(author);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AuthorsEditModel model) {
            var isValidation = await _validator.ValidateAsync(model);

            if (!isValidation.IsValid) {
                isValidation.AddToModelState(ModelState);
            }

            if (!ModelState.IsValid) {
                return View(model);
            }

            var author = model.Id > 0
                ? await _blogRepository.FindAuthorByIdAsync(model.Id) : null;

            if (author == null) {
                author = _mapper.Map<Author>(model);
                author.Id = 0;
                author.JoinedDate = DateTime.Now;
            }
            else {
                _mapper.Map(model, author);
            }

            await _blogRepository.AddOrEditAuthorAsync(author);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteAuthor(int id) {
            var post = await _blogRepository.FindAuthorByIdAsync(id);
            await _blogRepository.DeleteAuthorByIdAsync(post.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
