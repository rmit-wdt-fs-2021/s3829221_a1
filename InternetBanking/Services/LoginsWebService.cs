using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Models;
using Managers;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public static class LoginsWebService
    {
        public static async Task ReadAndSaveLogin(string connectionString)
        {
            // Check if any login info exists in the table
            var loginManager = new LoginManager(connectionString);
            loginManager.Instantiate();
            if (Container.Logins.Any())
                return;

            // Contact web service and read JSON file
            using var client = new HttpClient();
            var getJson = client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/");
            var json = await getJson;

            // Deserialise JSON file into objects
            var logins = JsonConvert.DeserializeObject<List<Login>>(json);

            // Insert data stored in JSON into database
            foreach (var login in logins)
            {
                loginManager.InsertLogin(login);
            }
        }
    }
}
