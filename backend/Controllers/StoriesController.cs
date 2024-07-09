

using Microsoft.AspNetCore.Mvc;

public class StoriesController : Controller
{
    private readonly StoryService _storyService;
    public StoriesController(StoryService storyService)
    {
        _storyService = storyService;
    }


    [HttpGet("/api/v1/all-stories")]
    public async Task<IActionResult> GetAllStories()
    {
        var token = JwtService.VerifyToken(Request.Cookies["jwt"]!);
        if (token is default(int))
            return StatusCode(409, "unauthorized token");

        List<StoryFullDto>? stories = await _storyService.GetAllStories();
        if (stories?.Count == 0)
            return StatusCode(404, new {status=false, message="there is no stories"});
        else
            return StatusCode(200, new {status=true, stories=stories});
    }

    [HttpGet("/api/v1/stories")]
    public IActionResult GetAllUserStories()
    {
        var token = JwtService.VerifyToken(Request.Cookies["jwt"]!);
        if (token is default(int))
            return StatusCode(409, "unauthorized token");

        List<StoryFullDto>? stories = _storyService.GetAllUserStories(token);
        if (stories?.Count == 0)
            return StatusCode(404, new {status=false, message="there is no stories"});
        else
            return StatusCode(200, new {status=true, stories=stories});
    }

    [HttpPost("/api/v1/stories")]
    public async Task<IActionResult> CreateStory()
    {
        var token = JwtService.VerifyToken(Request.Cookies["jwt"]!);
        if (token is default(int))
            return StatusCode(409, "unauthorized token");

        var data = await Request.ReadFormAsync();
        var body = data.ToDictionary(b => b.Key, b => b.Value);
        var photo = data.Files.GetFile("Photo");
        var story = await _storyService.AddStoryAsync(token, body, photo!);
        if (story is null)
            return StatusCode(500, new {status=false, message="internal server error"});
        else
            return StatusCode(200, new {status=true, story=story});
    }

    [HttpGet("/api/v1/story/{id}")]
    public async Task<IActionResult> GetStoryById(int id)
    {
        var token = JwtService.VerifyToken(Request.Cookies["jwt"]!);
        if (token is default(int))
            return StatusCode(409, "unauthorized token");

        var story = await _storyService.GetStoryById(id);
        if (story is null)
            return StatusCode(404, new {status=false, message=$"there is no story with this id {id}"});
        else
            return StatusCode(200, new {status=true, story=story});
    }
}
