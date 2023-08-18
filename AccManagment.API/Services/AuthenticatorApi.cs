namespace AccManagment.API.Services;

public class AuthenticatorApi
{
    public async Task<bool> ValidateGoogleAuthenticatorCodeAsync(string userEnteredCode, string secretCode)
    {
        using (HttpClient client = new HttpClient())
        {
            var validationUrl = $"https://www.authenticatorApi.com/Validate.aspx?Pin={userEnteredCode}&SecretCode={secretCode}";
            var response = await client.GetStringAsync(validationUrl);

            return response.Equals("True", StringComparison.OrdinalIgnoreCase);
        }
    }
    public async Task<string> PairWithGoogleAuthenticatorAsync( string secretCode)
    {
        string appName = "Rati's App";
        string appInfo = " Authentication";
        using (HttpClient client = new HttpClient())
        {
            var url = $"https://www.authenticatorApi.com/pair.aspx?AppName={appName}&AppInfo={appInfo}&SecretCode={secretCode}";
            var response = await client.GetStringAsync(url);
            return response;  
        }
    }
}