namespace Unistream_T4.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string? message = null)
           : base(message)
        {

        }
    }
}
