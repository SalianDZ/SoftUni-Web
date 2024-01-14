using Forum.Services.Interfaces;
using Forum.ViewModels.Post;
using ForumApp.ViewModels.Post;
using Microsoft.AspNetCore.Mvc;

namespace ForumApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService; 
        }

        public async Task<IActionResult> All()
        { 
            IEnumerable<PostListViewModel> allPosts = await postService.ListAllAsync();
            return View(allPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(PostAddFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
				await postService.AddPostAsync(model);
			}
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occured while adding your post");
                return View(model);
            }
            
            return RedirectToAction("All");
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
				PostAddFormModel model = await postService.GetForEditByIdAsync(id);
				return View(model);
			}
            catch (Exception)
            {
                return RedirectToAction("All", "Post");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, PostAddFormModel editedModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editedModel);
            }

            try
            {
                await postService.EditByIdAsync(id, editedModel);
            }
            catch (Exception)
            {
				ModelState.AddModelError(string.Empty, "Unexpected error occured while editing your post");
				return View(editedModel);
			}

            return RedirectToAction("All");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await postService.DeleteByIdAsync(id);
            }
            catch (Exception)
            {

            }

            return RedirectToAction("All", "Post");
        }
    }
}
