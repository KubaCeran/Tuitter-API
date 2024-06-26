﻿using AutoMapper;
using Core.DTOs.Posts;
using Core.Entities;
using Core.Options.Pagination;
using Infrastructure.Middlewares.Exceptions;
using Infrastructure.Repositories.Categories;
using Infrastructure.Repositories.Posts;

namespace Infrastructure.Services.Posts
{
    public class PostService(
        IPostRepository postRepository,
        ICategoryRepository categoryRepository,
        IMapper mapper) : IPostService
    {
        public async Task AddPost(CreatePostDto postDto, int userId, CancellationToken cancellationToken)
        {
            await ValidatePostDto(postDto, cancellationToken);

            var categories = categoryRepository.FindCategoryByName(postDto.Categories!);
            var postEntity = mapper.Map<Post>(postDto);

            postEntity.UserId = userId;
            postEntity.Categories = categories.ToList();

            await postRepository.AddAsync(postEntity, cancellationToken);
        }

        public async Task DeletePost(int postId, int userId, CancellationToken cancellationToken)
        {
            var post = await postRepository.GetByIdAsync(postId, false, cancellationToken);
            if (post.UserId == userId)
                await postRepository.DeleteByIdAsync(postId, cancellationToken);
            else
                throw new ForbiddenException("Cannot delete someones else post");
        }

        public PagedList<PostDto> GetAllPostsByParentId(PaginationOptions paginationOptions, int? parentPostId)
        {
            var postsQuery = postRepository
                .GetAllPostsWithCategoriesAndReplies()
                .Where(x => x.ParentPostId == parentPostId)
                .OrderBy(x => x.CreationTime);

            var posts = mapper.ProjectTo<PostDto>(postsQuery);
            return PagedList<PostDto>.Create(posts, paginationOptions);
        }

        public PagedList<PostDto> GetAllPostsForCategory(PaginationOptions paginationOptions, string categoryName)
        {
            var postsQuery = postRepository.GetAllPostsByCategoryName(categoryName).OrderBy(x => x.CreationTime);
            var posts = mapper.ProjectTo<PostDto>(postsQuery);
            return PagedList<PostDto>.Create(posts, paginationOptions);
        }

        public PagedList<PostDto> GetAllPostsForUser(PaginationOptions paginationOptions, int userId)
        {
            var postsQuery = postRepository.GetAllPostsByUserId(userId).OrderBy(x => x.CreationTime);
            var posts = mapper.ProjectTo<PostDto>(postsQuery);
            return PagedList<PostDto>.Create(posts, paginationOptions);
        }

        private async Task ValidatePostDto(CreatePostDto postDto, CancellationToken cancellationToken)
        {
            if(postDto.ParentPostId is null && postDto.Categories is null)
                throw new BadRequestException("No categories specified");

            if(postDto.ParentPostId is not null)
            {
                await postRepository.GetByIdAsync(Convert.ToInt32(postDto.ParentPostId), false, cancellationToken); //throws if there is no parent post with given id
                postDto.Categories = [];
            }
        }
    }
}
