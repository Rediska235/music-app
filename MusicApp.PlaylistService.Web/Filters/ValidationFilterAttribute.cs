using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Application.Validators;

namespace MusicApp.PlaylistService.Web.Filters;

public class ValidationFilterAttribute : ActionFilterAttribute
{
    private readonly PlaylistInputDtoValidator _validator;

    public ValidationFilterAttribute()
    {
        _validator = new();
    }
    
    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
        object playlistObj;
        actionContext.ActionArguments.TryGetValue("playlistInputDto", out playlistObj);
        var playlist = (PlaylistInputDto)playlistObj;

        _validator.ValidateAndThrow(playlist);
    }
}