
using Microsoft.AspNetCore.Mvc;

public class NotificationController : Controller
{
    private readonly NotificationService _notificationService;

    public NotificationController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("/api/v1/notifications")]
    public async Task<IActionResult> GetAllNotification()
    {
        var token = Request.Cookies["jwt"];
        if (token is null)
            return Unauthorized();
        
        var result = await _notificationService.GetAllNotifications(token);
        return new JsonResult(new {status=true, notifications=new {postNotification=result.Item1, friendRequestNotifications=result.Item2}});
    }
}