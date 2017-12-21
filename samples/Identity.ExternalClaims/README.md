AuthSamples.Identity.ExternalClaims
=================

Sample demonstrating copying over external claims from Google authentication during login:

Steps:
1. Configure a google OAuth2 project via: https://console.developers.google.com/apis/credentials with the appropriate
   JavaScript origins(i.e. https://localhost:44343) and redirectURIs (https://localhost:44343/signin-google).
2. Update Startup.cs AddGoogle()'s options with ClientId and ClientSecret for your google app.
3. Run the app and click on the MyClaims tab, this should trigger a redirect to login.
4. Login via the Google button, this should redirect you to google.
3. You should be redirected back to /Home/MyClaims which will output the user claims.  Notice that an AccessToken claim is included.

How this works:
- Startup hooks the OnCreatingTicket event to copy over the AccessToken claim from the google identity which is stored in the Identity.ExternalCookie.
- In ExternalLogin.cshtml.cs when a user is registered from an external login, we copy the access token into the user claims in OnPostConfirmationAsync.
- This results in the AccessToken being stored in the user's row. Note: this sample does not update the access token on subsequent logins. You do
  get access to the current token on every login, so the user's claim could be updated in OnGetCallbackAsync if desired.