namespace HouseBrokerMVP.Business.Exceptions
{
    public class InvalidCredentials : Exception
    {
        public InvalidCredentials()
            : base()
        {
        }

        public InvalidCredentials(string message)
            : base(message)
        {
        }

        public InvalidCredentials(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
