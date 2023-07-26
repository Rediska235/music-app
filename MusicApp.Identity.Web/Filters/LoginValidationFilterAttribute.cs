using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using MusicApp.Identity.Application.DTOs;
using MusicApp.Identity.Application.Validators;

namespace MusicApp.Identity.Web.Filters;

public class LoginValidationFilterAttribute : ActionFilterAttribute
{
    private readonly UserLoginDtoValidator _validator;

    public LoginValidationFilterAttribute()
    {
        _validator = new();
    }
    
    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
        object playlistObj;
        actionContext.ActionArguments.TryGetValue("userLoginDto", out playlistObj);
        var playlist = (UserLoginDto)playlistObj;

        _validator.ValidateAndThrow(playlist);
    }
}