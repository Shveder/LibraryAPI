namespace Library.Infrastructure.Exceptions;

public class EntityNotFoundException(string message) : Exception(message);