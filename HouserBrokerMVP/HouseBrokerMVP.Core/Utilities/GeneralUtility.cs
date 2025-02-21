namespace HouseBrokerMVP.Core.Utilities
{
    
    public class GeneralUtility
    {
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now.ToLocalTime();
        }
        public static string GetUniqueFileIdentifier()
        {
            return DateTime.Now.ToString("yyyyMMddhhmmssffff");
        }
    }
}
