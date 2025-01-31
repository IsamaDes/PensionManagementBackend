namespace Pension.Domain.Exceptions;
public class NotFoundException(string resource, string identifier) : Exception($"The {resource} with id: {identifier} doesn't exist.")
{

}