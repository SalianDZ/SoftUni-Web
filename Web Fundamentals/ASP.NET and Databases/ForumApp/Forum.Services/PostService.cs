using Forum.Services.Interfaces;
using Forum.ViewModels.Post;
using ForumApp.Data;
using ForumApp.Data.Models;
using ForumApp.ViewModels.Post;
using Microsoft.EntityFrameworkCore;

namespace Forum.Services
{
	public class PostService : IPostService
    {
        private readonly ForumDbContext dbContext;
        public PostService(ForumDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task AddPostAsync(PostAddFormModel model)
		{
            Post post = new Post 
            {
                Title = model.Title,
                Content = model.Content
            };

            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
		}

		public async Task DeleteByIdAsync(string id)
		{
            Post postToDelete = await dbContext.Posts.FirstAsync(p => p.Id.ToString() == id);

            dbContext.Posts.Remove(postToDelete);
            await dbContext.SaveChangesAsync();
        }

        public async Task EditByIdAsync(string id, PostAddFormModel editedModel)
		{
            Post postToEdit = await dbContext.Posts.FirstAsync(p => p.Id.ToString().ToLower() == id.ToLower());

            postToEdit.Title = editedModel.Title;   
            postToEdit.Content = editedModel.Content;

            await dbContext.SaveChangesAsync();
		}

		public async Task<PostAddFormModel> GetForEditByIdAsync(string id)
		{
            Post postToEdit = await dbContext.Posts
                .FirstAsync(p => p.Id.ToString().ToLower() == id.ToLower()); 

            return new PostAddFormModel()
            { 
                Title = postToEdit.Title,
                Content = postToEdit.Content
            };
		}

		public async Task<IEnumerable<PostListViewModel>> ListAllAsync()
        {
            IEnumerable<PostListViewModel> allPosts = await dbContext.Posts.Select(p => new PostListViewModel()
            {
                Id = p.Id.ToString(),
                Title = p.Title,
                Content = p.Content
            })
                .ToArrayAsync();

            return allPosts;
        }
    }
}
