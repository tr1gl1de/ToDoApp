using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ToDoApp.Entities.Helpers;
using ToDoApp.Entities.Models;

namespace ToDoApp.WebApi.Controllers;

public abstract class BaseController : ControllerBase
{
    protected Guid GetAuthUserId()
    {
        var subClaim = User.Claims.Single(claim => claim.Type == "sub");
        var userId = Guid.Parse(subClaim.Value);
        return userId;
    }

    protected void AddPaginationInHeaders(PagedList<Note> userNotes)
    {
        var metadata = new
        {
            userNotes.CurrentPage,
            userNotes.HasNext,
            userNotes.HasPrevious,
            userNotes.PageSize,
            userNotes.TotalCount,
            userNotes.TotalPages
        };
        
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
    }
}