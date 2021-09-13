namespace ElvyAuthorizationSDK
{
    public class ElvyAccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string result { get; set; }
    }
}
