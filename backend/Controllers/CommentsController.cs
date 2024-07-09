

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

public class CommentsController : Controller
{
    private readonly CommentService _commentService;
    public CommentsController(CommentService commentService)
    {
        _commentService = commentService;
    }


    [HttpPost("/api/v1/comments")]
    public async Task<IActionResult> CreateComment()
    {
        var token = Request.Cookies["jwt"];
        if (token is null)
            return Unauthorized(new {status=false, message="unauthorized"});
        
        var form = await new FormReader(Request.Body).ReadFormAsync();

        if (form is null)
            return BadRequest(new {status=false, message="form is null"});
        
        CommentDto? comment = await _commentService.CreateComment(token, form);
        if (comment is null)
            return StatusCode(500, new {status=false, message="server didn't create comment"});
        return Ok(new {status=true, comment=comment});
    }

    [HttpDelete("/api/v1/comment/{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var token = Request.Cookies["jwt"];
        if (token is null)
            return Unauthorized(new {status=false, message="unauthorized"});
        
        var result = await _commentService.DeleteComment(token, id);
        if (result is false)
            return StatusCode(500, new {status=false, message="error in deleting comment"});
        return Ok(new {status=true, message="comment deleted !"});
    }
}

