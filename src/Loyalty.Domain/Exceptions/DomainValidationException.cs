namespace Loyalty.Domain.Exceptions;

public class DomainValidationException(string msg) : Exception(msg);