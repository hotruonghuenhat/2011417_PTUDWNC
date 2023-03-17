using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Areas.Admin.Controllers {
    public class PostsController : Controller {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly IMediaManager _mediaManager;
        private readonly IValidator<PostEditModel> _postValidator;
        private readonly ILogger<PostsController> _logger;

        public PostsController(ILogger<PostsController> logger, IBlogRepository blogRepository, IMediaManager mediaManager, IMapper mapper) {
            _logger = logger;
            _blogRepository = blogRepository;
            _mediaManager = mediaManager;
            _mapper = mapper;
            // Khởi tạo validator to post
            _postValidator = new PostValidator(blogRepository);
        }
        public async Task<IActionResult> Index(PostFilterModel model) {

            _logger.LogInformation("Tạo điều kiện truy vấn");
            // using library map :D --> fast, concise: gọn
            var postQuery = _mapper.Map<PostQuery>(model);

            _logger.LogInformation("Lấy danh sách bài viết từ CSDL");

            ViewBag.PostsList = await _blogRepository.GetPagedPostsAsync(postQuery, 1, 10);

            _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");
            await PopulatePostFilterModeAsync(model);

            return View(model);
        }

        private async Task PopulatePostFilterModeAsync(PostFilterModel model) {
            var authors = await _blogRepository.GetAuthorsAsync();
            var categories = await _blogRepository.GetCategoriesAsync();

            model.AuthorList = authors.Select(a => new SelectListItem() {
                Text = a.FullName,
                Value = a.Id.ToString(),
            });

            model.CategoryList = categories.Select(c => new SelectListItem() {
                Text = c.Name,
                Value = c.Id.ToString(),
            });
        }

        private async Task PopulatePostEditModelAsync(PostEditModel model) {
            var authors = await _blogRepository.GetAuthorsAsync();
            var categories = await _blogRepository.GetCategoriesAsync();

            model.AuthorList = authors.Select(a => new SelectListItem() {
                Text = a.FullName,
                Value = a.Id.ToString()
            });

            model.CategoryList = categories.Select(s => new SelectListItem() {
                Text = s.Name,
                Value = s.Id.ToString()
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0) {
            // ID = 0 => Thêm bài viết mới
            // ID > 0 => Cập nhật bài viết
            var post = id > 0 ? await _blogRepository.GetPostByIdAsync(id, true) : null;

            // tạo view model từ dữ liệu của bài viết
            var model = post == null ? new PostEditModel() : _mapper.Map<PostEditModel>(post);

            // gán giá trị khác cho view model
            await PopulatePostEditModelAsync(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostEditModel model) {
            var validationResult = await _postValidator.ValidateAsync(model);
            if (!validationResult.IsValid) {
                validationResult.AddToModelState(ModelState);
            }

            if (!ModelState.IsValid) {
                await PopulatePostEditModelAsync(model);
                return View(model);
            }

            var post = model.Id > 0
                ? await _blogRepository.GetPostByIdAsync(model.Id) : null;


            if (post == null) {
                post = _mapper.Map<Post>(model);
                post.Id = 0;
                post.PostedDate = DateTime.Now;
            }
            else {
                //_mapper.Map<Post>(post);
                _mapper.Map(model, post);
                post.Category = null;
                post.ModifiedDate = DateTime.Now;
            }

            // Nếu người dùng có upload hình ảnh minh họa cho bài viết
            if (model.ImageFile?.Length > 0) {
                // Thì thực hiện lưu vào thư mục uploads
                var newImagePath = await _mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(), model.ImageFile.FileName, model.ImageFile.ContentType);

                // Nếu thành công, xóa hình ảnh cũ nếu có
                if (!string.IsNullOrEmpty(newImagePath)) {
                    await _mediaManager.DeleteFileAsync(post.ImageUrl);
                    post.ImageUrl = newImagePath;
                }
            }

            await _blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags());

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> VerityPostSlug(int id, string urlSlug) {
            var slugIsExisted = await _blogRepository.IsPostSlugExistedAsync(id, urlSlug);

            return slugIsExisted ? Json($"Slug: '{urlSlug}' đã được sử dụng")
                                   : Json(true);

        }
    }
}
