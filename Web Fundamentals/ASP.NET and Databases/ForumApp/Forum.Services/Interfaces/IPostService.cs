using Forum.ViewModels.Post;
using ForumApp.ViewModels.Post;

namespace Forum.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostListViewModel>> ListAllAsync();

        Task AddPostAsync(PostAddFormModel model);

        Task<PostAddFormModel> GetForEditByIdAsync(string id);

        Task EditByIdAsync(string id, PostAddFormModel editedModel);

        Task DeleteByIdAsync(string id);
    }
}
