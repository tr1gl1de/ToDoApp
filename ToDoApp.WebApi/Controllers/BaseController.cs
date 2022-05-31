using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.WebApi.Controllers;

public abstract class BaseController : ControllerBase
{
    protected Guid GetAuthUserId()
    {
        var subClaim = User.Claims.Single(claim => claim.Type == "sub");
        var userId = Guid.Parse(subClaim.Value);
        return userId;
    }
}