namespace UserService.Domain.Exceptions;

public class CurrencyAlreadyInFavoriteException(int id)
    : Exception($"Currency with id {id} already in favorites.");
