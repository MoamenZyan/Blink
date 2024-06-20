

using Microsoft.AspNetCore.Mvc;

public class ReactionsController : Controller
{
    private readonly ReactionService _reactionService;
    public ReactionsController(ReactionService reactionService)
    {
        _reactionService = reactionService;
    }

    [HttpGet("/api/v1/reaction-post/{id}")]
    public async Task<IActionResult> AddReactionOnPost(int id)
    {
        var token = JwtService.VerifyToken(Request.Cookies["jwt"]!);
        if (token is default(int))
            return StatusCode(409, "unauthorized user");

        bool result = await _reactionService.AddReactionOnPost(id, token);
        if (result is true)
            return Ok("reaction added on a post");
        else
            return BadRequest("something wrong happend");
    }

    [HttpDelete("/api/v1/reaction-post/{id}")]
    public async Task<IActionResult> DeleteReactionFromPost(int id)
    {
        var token = JwtService.VerifyToken(Request.Cookies["jwt"]!);
        if (token is default(int))
            return StatusCode(409, "unauthorized user");

        bool result = await _reactionService.DeleteReactionFromPost(id, token);
        if (result is true)
            return Ok("reaction deleted from the post");
        else
            return BadRequest("something wrong happend");
    }

    [HttpGet("/api/v1/post/has-reaction/{id}")]
    public IActionResult HasReactionOnPost(int id)
    {
        var token = JwtService.VerifyToken(Request.Cookies["jwt"]!);
        if (token is default(int))
            return StatusCode(409, "unauthorized user");

        var result = _reactionService.HasReactionOnAPost(id, token);
        if (result is true)
            return Ok("has reaction on this post");
        else
            return StatusCode(404, "reaction not found");
    }
}
