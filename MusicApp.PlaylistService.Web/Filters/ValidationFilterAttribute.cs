using FluentValidation;
using MassTransit.Serialization;
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
        actionContext.ActionArguments.TryGetValue("playlistInputDto", out PlaylistInputDto? playlistInputDto);
        if (playlistInputDto != null)
        {
            _validator.ValidateAndThrow(playlistInputDto);
        }
    }
}