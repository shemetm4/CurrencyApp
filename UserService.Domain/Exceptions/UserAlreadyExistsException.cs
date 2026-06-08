namespace UserService.Domain.Exceptions;

public class UserAlreadyExistsException(string name)
    : Exception($"User with name {name} already exists.");
