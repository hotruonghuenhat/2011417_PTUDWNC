using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations {
    public class AuthorValidator : AbstractValidator<AuthorsEditModel> {
        private readonly IBlogRepository blogRepository;

        public AuthorValidator(IBlogRepository blogRepository) {
            this.blogRepository = blogRepository;

            // Validator
            RuleFor(a => a.FullName)
                .NotEmpty().WithMessage("Không được để trống tên giả");

            RuleFor(a => a.Email)
                .NotEmpty().WithMessage("Không được để trống email")
                .MaximumLength(150).WithMessage("Email tối đa 150 ký tự");

            RuleFor(a => a.Notes)
                .NotEmpty().WithMessage("Không được để trống")
                .MaximumLength(1000).WithMessage("Ghi chú không được quá 1000 ký tự");

            RuleFor(x => x.UrlSlug)
               .NotEmpty()
               .WithMessage("Slug không được để trống")
               .MaximumLength(1000)
               .WithMessage("Slug không được nhiều hơn 1000 ký tự");

            RuleFor(x => x.UrlSlug)
                .MustAsync(async (authorModel, slug, cancellationToken) => !await blogRepository
                    .IsAuthorSlugExistedAsync(authorModel.Id, slug, cancellationToken))
                    .WithMessage("Slug '{PropertyValue}' đã được sử dụng");
        }
    }
}
