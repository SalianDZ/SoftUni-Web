using ForumApp.Data.Models;

namespace ForumApp.Data.Seeding
{
    class PostSeeder
    {
        internal Post[] GeneratePosts()
        {
            ICollection<Post> posts = new HashSet<Post>();
            Post currentPost;

            currentPost = new Post()
            {
                Title = "My first post",
                Content = "Explore diverse posts on our app—inspiring stories, helpful tips, and entertaining moments await. Discover a world of content today!"
            };

            posts.Add(currentPost);

            currentPost = new Post()
            {
                Title = "My second post",
                Content = "Discover a myriad of posts in our app—each a gem of wisdom, a burst of joy. Explore diverse content daily!"
            };
            posts.Add(currentPost);

            currentPost = new Post()
            {
                Title = "My third post",
                Content = "Dive into a tapestry of posts on our app—uncover joy, insights, and surprises. Your daily dose of delightful content awaits!"
            };
            posts.Add(currentPost);

            return posts.ToArray();
        }
    }
}
