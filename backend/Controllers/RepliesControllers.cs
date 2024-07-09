

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

public class RepliesController : Controller
{
    private readonly ReplyService _replyService;

    public RepliesController(ReplyService replyService)
    {
        _replyService = replyService;
    }

    [HttpPost("/api/v1/replies")]
    public async Task<IActionResult> CreateReply()
    {
        var token = Request.Cookies["jwt"];
        if (token is null)
            return Unauthorized(new {status=false, message="unauthorized"});
        var body = await new FormReader(Request.Body).ReadFormAsync();
        if (body is null)
            return BadRequest(new {status=false, message="body is null"});
        
        var result = await _replyService.AddReply(token, body);
        if (result is null)
            return StatusCode(500, new {status=false, message="error in creating reply"});
        return Ok(new {status=true, reply=result, message="reply created !"});
    }

    [HttpGet("/api/v1/comment/{id}/replies")]
    public IActionResult GetAllCommentReplies(int id)
    { 
        try
        {
            var result = _replyService.GetAllCommentReplies(id);
            return Ok(new {status=true, replies=result});
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new{status=false, message="error in retrieving the replies"});
        }
    }

    [HttpDelete("/api/v1/reply/{id}")]
    public async Task<IActionResult> DeleteReply(int id)
    {
        var token = Request.Cookies["jwt"];
        if (token is null)
            return Unauthorized(new {status=false, message="unauthorized"});
        
        var result = await _replyService.DeleteReply(token, id);
        if (result is false)
            return StatusCode(500, new {status=false, message="error in deleting reply"});
        return Ok(new {status=true, message="reply deleted !"});
    }
}
