# ElvyAuthorization.NET
Elvy Authorization .NET SDK

## Hoe werkt het:
De Elvy Authorization SDK is een .NET library die je kunt gebruiken om de Elvy token management voor je te laten doen. De library gebruikt je refresh token om automatisch access tokens aan te vragen bij de Elvy Licenser wanneer jij die nodig hebt. 




## Hoe gebruik je de SDK:
 - ### Stap 1: Download de SDK.


 - ### Stap 2: importeer het project in je .NET solution.


 - ### Stap 3: Voeg het volgende toe aan je appsettings.json:
   `"ElvyRefreshToken": "Vervang dit met je eigen refresh token`

 - ### Stap 4: Initialiseer met dependency injection de TokenStorage in je startup.cs.
    - Dit is mogelijk door de volgende lijn aan je `ConfigureServices` methode toe te voegen:
      `services.AddSingleton(x => new TokenStorage("https://localhost:44386/v1/Token/Refresh", Configuration["ElvyRefreshToken"]));`

 - ### Stap 5: Gebruik de geinjecteerde dependency in je controller.
   - Voeg het volgende toe aan de constructor van je controller:
     `TokenStorage tokenStorage`. In de meest basic vorm zou je controller er dan zo uit zien:
       ```
        private readonly TokenStorage _tokenStorage;

        public HomeController(TokenStorage tokenStorage)
        {
            _tokenStorage = tokenStorage;
        }
        ```

- ### Stap 6: Gebruik de tokens.
  - Dit kan door de volgende code aan te roepen:
    ```
    ElvyAccessToken elvyAccessToken = _tokenStorage.GetAccessToken();
    HttpRequestMessage request = new(HttpMethod.Get, "Gewenste elvy product url");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", elvyAccessToken.access_token);
    ```
