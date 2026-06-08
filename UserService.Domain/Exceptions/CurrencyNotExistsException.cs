namespace UserService.Domain.Exceptions;

public class CurrencyNotExistsException(int id)
    : Exception($"Currency with id {id} was not found.");

