using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Areas.Admin.Controllers;

public class TagsController : Controller {
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<TagEditModel> _validator;

    public TagsController(IBlogRepository blogRepository, IMapper mapper) {
        _blogRepository = blogRepository;
        _mapper = mapper;
        _validator = new TagValidator(_blogRepository);
    }

    public async Task<IActionResult> Index(TagFilterModel filter) {
        var query = _mapper.Map<TagQuery>(filter);
        var tags = await _blogRepository.GetListTagItemAsync(query);
        ViewBag.Tags = tags;

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id) {
        var tag = id > 0
            ? await _blogRepository.FindTagById(id)
            : null;

        var model = tag == null
            ? new TagEditModel()
            : _mapper.Map<TagEditModel>(tag);

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(TagEditModel model) {
        var isValidation = await _validator.ValidateAsync(model);

        if (!isValidation.IsValid) {
            isValidation.AddToModelState(ModelState);
        }

        if (!ModelState.IsValid) {
            return View(model);
        }

        var tag = model.Id > 0
            ? await _blogRepository.FindTagById(model.Id) : null;

        if (tag == null) {
            tag = _mapper.Map<Tag>(model);
            tag.Id = 0;
        }
        else {
            _mapper.Map(model, tag);
        }

        await _blogRepository.AddOrEditTagAsync(tag);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteTags(int id) {
        var post = await _blogRepository.FindTagById(id);
        await _blogRepository.DeleteTagByIdAsync(post.Id);
        return RedirectToAction(nameof(Index));
    }
}
