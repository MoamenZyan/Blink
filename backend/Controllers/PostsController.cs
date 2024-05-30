
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

public class PostsController : Controller
{
    private readonly PostService _postService;
    public PostsController(PostService postService)
    {
        _postService = postService;
    }

    /// <summary>
    /// To create post
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/v1/posts
    ///     {
    ///         "user_id": "1",
    ///         "caption": "Hello Guys !!",
    ///         "privacy": "public",
    ///     }
    ///
    /// </remarks>
    /// <param name="data">JSON data that has post info</param>
    /// <response code="201">Post Created</response>
    /// <response code="400">Json is null</response>
    /// <response code="401">Unauthenticated user / Invalid or expired JWT token</response>
    /// <response code="500">Internal server error</response>
    [HttpPost("/api/v1/posts")]
    public async Task<IActionResult> CreatePost([FromBody] Object data)
    {
        try
        {
            if (data is null)
                return BadRequest(new {status = false, message = "JSON data is null"});
            var json = JObject.Parse(data.ToString()!);
            Console.WriteLine(json);
            if (Request.Cookies["jwt"] is null)
                return Unauthorized(new {status = false, message = "Unauthenticated user"});
            
            var userId = JwtService.VerifyToken(Request.Cookies["jwt"]!);
            if (userId is default(int))
                return Unauthorized(new { status = false, message = "Invalid or expired JWT token" });

            Post? post = await _postService.AddPost(userId, json);
            return StatusCode(201, new {status=true, message="Post created successfully", post=post});
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return StatusCode(500, new {status=false, mesage="internal server error"});
        }
    }
    
    /// <summary>
    /// To Get All Posts
    /// </summary>
    /// <response code="200">Got all posts</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("/api/v1/posts")]
    public async Task<ActionResult> GetAllPosts()
    {
        try
        {
            var posts = await _postService.GetAllPosts();
            return Ok(new {status=true, posts=posts});
        }
        catch(Exception exp)
        {
            Console.WriteLine(exp);
            return StatusCode(500, new {status=false, mesage="internal server error"});
        }
    }

    /// <summary>
    /// Get Specific Post By Id
    /// </summary>
    /// <param name="id">Id of the post</param>
    /// <response code="200">Found the post</response>
    /// <response code="404">No post found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("/api/v1/post/{id}")]
    public async Task<ActionResult> GetPost(int id)
    {
        try
        {
            var post = await _postService.GetPostById(id);
            if (post is null)
                return NotFound(new {status=false, message=$"no post found with the id {id}"});
            return Ok(new {status=true, post=post});
        }
        catch(Exception exp)
        {
            Console.WriteLine(exp);
            return StatusCode(500, new {status=false, mesage="internal server error"});
        }
    }

    /// <summary>
    /// To Update Specific Post
    /// </summary>
    /// <param name="data">New post info</param>
    /// <param name="id">Post id</param>
    /// <response code="200">Post updated successfully</response>
    /// <response code="401">Unauthenticated user</response>
    /// <response code="500">Internal server error</response>
    [HttpPatch("/api/v1/post/{id}")]
    public async Task<ActionResult> UpdatePost([FromBody] Object data, int id)
    {
        try
        {
            if (data is null)
                return BadRequest(new {status = false, message = "JSON data is null"});
            
            var json = JObject.Parse(data.ToString()!);
            if (Request.Cookies["jwt"] is null)
                return Unauthorized(new {status = false, message = "Unauthenticated user"});

            var result = await _postService.UpdatePost(json, Request.Cookies["jwt"]!, id);
            if (result)
                return Ok(new {status=true, message="post updated successfully"});
            else
                return StatusCode(500, new {status=false, mesage="internal server error"});
        }
        catch(Exception exp)
        {
            Console.WriteLine(exp);
            return StatusCode(500, new {status=false, mesage="internal server error"});
        }
    }


    /// <summary>
    /// To Delete Specific Post
    /// </summary>
    /// <param name="id">Post id</param>
    /// <response code="200">Post deleted successfully</response>
    /// <response code="401">Unauthenticated user</response>
    /// <response code="500">Internal server error</response>
    [HttpDelete("/api/v1/post/{id}")]
    public async Task<ActionResult> DeletePost(int id)
    {
        try
        {
            if (Request.Cookies["jwt"] is null)
                return Unauthorized(new {status = false, message = "Unauthenticated user"});
            
            var result = await _postService.DeletePost(Request.Cookies["jwt"]!, id);
            if (result)
                return Ok(new {status=true, message="post deleted successfully"});
            else
                return StatusCode(500, new {status=false, mesage="internal server error"});
        }
        catch(Exception exp)
        {
            Console.WriteLine(exp);
            return StatusCode(500, new {status=false, mesage="internal server error"});
        }
    }
}
