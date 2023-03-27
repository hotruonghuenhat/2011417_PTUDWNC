using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations {
    public class TagValidator : AbstractValidator<TagEditModel> {
        private readonly IBlogRepository blogRepository;

        public TagValidator(IBlogRepository blogRepository) {
            this.blogRepository = blogRepository;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Bạn không được để trống tên thẻ")
                .MaximumLength(50)
                .WithMessage("Tên thẻ không được nhiều hơn 50 ký tự");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Giới thiệu không được nhiều hơn 1000 ký tự");

            RuleFor(x => x.UrlSlug)
                .NotEmpty()
                .WithMessage("Bạn không được để trống slug")
                .MaximumLength(50)
                .WithMessage("Slug không được nhiều hơn 1000 ký tự");

            RuleFor(x => x.UrlSlug)
                .MustAsync(async (tagModel, slug, cancellationToken) => !await blogRepository
                    .IsTagSlugExistedAsync(tagModel.Id, slug, cancellationToken))
                    .WithMessage("Slug '{PropertyValue}' đã được sử dụng");
        }
    }
}
