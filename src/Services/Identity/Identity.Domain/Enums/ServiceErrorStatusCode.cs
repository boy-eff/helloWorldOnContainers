namespace Identity.Domain.Enums;

public enum ServiceErrorStatusCode
{
    WrongAction = 1,
    NotFound = 2,
    ForbiddenAction = 4,
    Conflict = 8
}