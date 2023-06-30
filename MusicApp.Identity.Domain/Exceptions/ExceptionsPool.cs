using System.Security.Authentication;

namespace MusicApp.Identity.Domain.Exceptions;

public static class Exceptions
{
    public static AlreadyExistsException usernameIsTaken = new("This username is taken.");

    public static InvalidCredentialException invalidCredential = new("Invalid username or password.");
    public static InvalidCredentialException invalidRefreshToken = new("Invalid refresh or access token.");
}
