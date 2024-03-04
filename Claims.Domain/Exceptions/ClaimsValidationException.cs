namespace Claims.Domain.Exceptions
{
    public class ClaimsValidationException : Exception
    {
        public ClaimsValidationException(string message) : base(message)
        {
        }
    }
}
