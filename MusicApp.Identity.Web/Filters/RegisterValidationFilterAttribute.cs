using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Application.Validators;

namespace MusicApp.Identity.Web.Filters;

public class RegisterValidationFilterAttribute : ActionFilterAttribute
{
    private readonly UserRegisterDtoValidator _validator;

    public RegisterValidationFilterAttribute()
    {
        _validator = new();
    }
    
    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
        object playlistObj;
        actionContext.ActionArguments.TryGetValue("userRegisterDto", out playlistObj);
        var playlist = (UserRegisterDto)playlistObj;

        _validator.ValidateAndThrow(playlist);
    }
}