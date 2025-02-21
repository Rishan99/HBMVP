namespace HouseBrokerMVP.Business.Exceptions
{
    public class GeneralException : Exception
    {
        public GeneralException()
            : base()
        {
        }

        public GeneralException(string message)
            : base(message)
        {
        }

        public GeneralException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
