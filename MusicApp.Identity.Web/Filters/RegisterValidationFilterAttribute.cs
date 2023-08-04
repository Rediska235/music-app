using FluentValidation;
using MassTransit.Serialization;
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
        actionContext.ActionArguments.TryGetValue("userRegisterDto", out UserRegisterDto? userRegisterDto);
        if(userRegisterDto != null)
        {
            _validator.ValidateAndThrow(userRegisterDto);
        }
    }
}