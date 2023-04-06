﻿using Mapster;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApi.Models.Authors;
using TatBlog.WebApi.Models.Posts;

namespace TatBlog.WebApi.Mapsters;

public class MapsterConfiguration : IRegister {
    public void Register(TypeAdapterConfig config) {
        config.NewConfig<Author, AuthorDto>();
        config.NewConfig<Author, AuthorItem>()
            .Map(dest => dest.PostCount,
            src => src.Posts == null ? 0 : src.Posts.Count);

        config.NewConfig<AuthorEditModel, Author>();
        config.NewConfig<Category, CategoryItem>()
            .Map(dest => dest.PostCount,
            src => src.Posts == null ? 0 : src.Posts.Count);

        config.NewConfig<Post, PostDto>();
        config.NewConfig<Post, PostDetail>();
    }
}