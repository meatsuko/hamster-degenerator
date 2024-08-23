
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;

class ProxyDTO
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

class  Client : HttpClient
{
    public string AppToken = "";
    public string PromoId = "";
    
    public string EqName;
    
    public override bool Equals(object obj)
    {
        if (this == obj) return true;
        if (obj == null || GetType() != obj.GetType()) return false;
        var that = (Client)obj;
        return EqName == that.EqName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(EqName);
    }


    public HttpClient HttpClient;

    public Client(HttpClientHandler httpClientHandler, string appToken, string promoId)
    {
        HttpClient = new HttpClient(httpClientHandler);
        this.AppToken = appToken;
        this.PromoId = promoId;
    }
    
    
}

class Programm
{
    
    public static List<WebProxy> __listWebProxy = new List<WebProxy>();
    
    public static void Main()
    {
        Console.WriteLine("Hamster Cumpot Promo Code DeGenerator v0.4.7 new game: Polysphere, Mow and Trim, Mud Racing");
        Console.WriteLine("Proxyable Promo Code Mining Tool ^_^ )))");
        Console.WriteLine("====");
        Console.WriteLine("Telegram bot: https://t.me/hamster_cumpot_bot/hamstercum?startapp=0");
        Console.WriteLine("====");
        Thread.Sleep(4 * 1000);

        Dictionary<string, string> GAMEZ = new Dictionary<string, string>();

        GAMEZ.Add("d1690a07-3780-4068-810f-9b5bbf2931b2", "b4170868-cef0-424f-8eb9-be0622e8e8e3"); //2048
        GAMEZ.Add("d28721be-fd2d-4b45-869e-9f253b554e50", "43e35910-c168-4634-ad4f-52fd764a843f"); //BIKE
        GAMEZ.Add("74ee0b5b-775e-4bee-974f-63e7f4d5bacb", "fe693b26-b342-4159-8808-15e3ff7f8767"); //CLONE
        GAMEZ.Add("82647f43-3f87-402d-88dd-09a90025313f", "c4480ac7-e178-4973-8061-9ed5b2e17954"); // TRAIN
        GAMEZ.Add("8d1cc2ad-e097-4b86-90ef-7a27e19fb833", "dc128d28-c45b-411c-98ff-ac7726fbaea4"); // MERGE
        GAMEZ.Add("61308365-9d16-4040-8bb0-2f4a4c69074c", "61308365-9d16-4040-8bb0-2f4a4c69074c"); // TWERK
        
        GAMEZ.Add("2aaf5aee-2cbc-47ec-8a3f-0962cc14bc71", "2aaf5aee-2cbc-47ec-8a3f-0962cc14bc71"); // Polysphere
        GAMEZ.Add("ef319a80-949a-492e-8ee0-424fb5fc20a6", "ef319a80-949a-492e-8ee0-424fb5fc20a6"); // Mow and Trim
        GAMEZ.Add("8814a785-97fb-4177-9193-ca4180ff9da8", "8814a785-97fb-4177-9193-ca4180ff9da8"); // Mud Racing
        
        try
        {
            string json = File.ReadAllText("proxy.json.txt");

            List<ProxyDTO> proxyList = JsonConvert.DeserializeObject<List<ProxyDTO>>(json);
            foreach (ProxyDTO proxy in proxyList)
            {
                __listWebProxy.Add(new WebProxy(proxy.Host){ Credentials = new NetworkCredential(proxy.Username, proxy.Password)});
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        if (__listWebProxy.Count == 0) throw new Exception("Proxy List ERROR");


        for (int i = 0; i < __listWebProxy.Count; i++)
        {
            var webProxy = __listWebProxy[i];

            var keyValuePair = GAMEZ.ElementAt(i % GAMEZ.Count);
            THREAD(webProxy, keyValuePair.Key, keyValuePair.Value);
        }
        
        
        
        

        while (true)
        {
            Thread.Sleep(5 * 1000);
        }
    }

    static void THREAD(WebProxy webProxy, string appToken, string promoId)
    {
        new Thread(() =>
        {
            while (true)
            {
                try
                {
                    List<Client> httpClientList = new List<Client>();
                    for (int i = 0; i < 20; i++)
                    {
                        try
                        {
                            var client = __createClient(webProxy, appToken, promoId);
                            httpClientList.Add(client);
                        }
                        catch (Exception e) { }
                    }
                    

                    Thread.Sleep(20 * 1000);

                    int attempts = 10;
                    while (httpClientList.Count > 0 || attempts > 0)
                    {
                        flusher:
                        List<Client> httpClientIterator = new List<Client>(httpClientList);
                        foreach (Client client in httpClientIterator)
                        {
                            string __pormoId = client.PromoId;
                            HttpClient httpClient = client.HttpClient;
                            try
                            {
                                StringContent requestBody;
                                JsonDocument jsonDocument;
                                JsonElement clientTokenElement;
                                string response, bearerToken;

                                requestBody = new StringContent(
                                    "{\"promoId\":\"" + __pormoId + "\",\"eventId\":\"" + GenerateEventId() +
                                    "\",\"eventOrigin\":\"undefined\"}",
                                    Encoding.UTF8,
                                    "application/json"
                                );
                                response = __post(httpClient, "https://api.gamepromo.io/promo/register-event",
                                    requestBody);

                                if (EnsureTimeOut(response)) goto flusher;

                                jsonDocument = JsonDocument.Parse(response);
                                if (jsonDocument.RootElement.TryGetProperty("hasCode", out clientTokenElement))
                                {
                                    bool hasCode = clientTokenElement.GetBoolean();
                                    if (hasCode)
                                    {
                                        requestBody = new StringContent(
                                            "{\"promoId\":\"" + __pormoId + "\"}",
                                            Encoding.UTF8,
                                            "application/json"
                                        );
                                        response = __post(httpClient, "https://api.gamepromo.io/promo/create-code",
                                            requestBody);

                                        //if (EnsureTimeOut(response)) continue;

                                        jsonDocument = JsonDocument.Parse(response);
                                        if (jsonDocument.RootElement.TryGetProperty("promoCode",
                                                out clientTokenElement))
                                        {
                                            StorePromoCode(clientTokenElement.GetString());
                                        }

                                        httpClientList.Remove(client);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                httpClientList.Remove(client);
                                goto flusher;
                            }
                        }

                        

                        Console.WriteLine(Thread.CurrentThread.Name + ": " + $"flusher: wait 20s :: {attempts}");
                        Thread.Sleep(20 * 1000);
                        attempts--;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(Thread.CurrentThread.Name + ": " +"error: cycle restart");
                }
            }
        }) {
            Name = webProxy.Address.Host,
            IsBackground = true
        }.Start();
    }

    static Client __createClient(WebProxy webProxy, string appToken, string promoId)
    {
        clientCreate:
        var client = new Client(new HttpClientHandler()
        {
            Proxy = webProxy,
            UseProxy = true
        }, appToken, promoId);
        
        HttpClient httpClient = client.HttpClient;
                                
        httpClient.DefaultRequestHeaders.Host = "api.gamepromo.io";
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (iPhone; CPU iPhone OS 17_5_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148");

        var requestBody = new StringContent(
            "{\"appToken\":\"" + appToken + "\",\"clientId\":\"" + GenerateClientId() +
            "\",\"clientOrigin\":\"deviceid\"}",
            Encoding.UTF8,
            "application/json"
        );
        var response = __post(httpClient, "https://api.gamepromo.io/promo/login-client", requestBody);
        
        if(EnsureTimeOut(response)) goto clientCreate;
            
        string bearerToken;
        var jsonDocument = JsonDocument.Parse(response);
        if (jsonDocument.RootElement.TryGetProperty("clientToken", out var clientTokenElement))
        {
            bearerToken = clientTokenElement.GetString();
            Console.WriteLine(Thread.CurrentThread.Name + ": " + $"Client Token: {bearerToken}");
        }
        else
        {
            throw new Exception("bearer error");
        }

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", bearerToken);
        
        return client;
    }

    static bool EnsureTimeOut(string responseBody)
    {
        var jsonDocument = JsonDocument.Parse(responseBody);
        if (!jsonDocument.RootElement.TryGetProperty("error_code", out var clientTokenElement)) return false;
    
        Console.WriteLine(Thread.CurrentThread.Name + ": " +"await 15s :: Time Out");
        Thread.Sleep(15 * 1000);
        return true;
    }
    
    
    static string __post(HttpClient httpClient, string url, StringContent requestBody)
    {
        // Настройка заголовков
        requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json")
        {
            CharSet = "utf-8"
        };

        try
        {
            var response =  httpClient.PostAsync(url, requestBody).GetAwaiter().GetResult();

            string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            //Console.WriteLine(responseBody);
            //response.EnsureSuccessStatusCode();
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(Thread.CurrentThread.Name + ": " + url + " error");
            throw e;
        }
    }

    static string GenerateClientId()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var random = new Random();
        var randomDigits = new char[19];

        for (int i = 0; i < 19; i++)
        {
            randomDigits[i] = (char)('0' + random.Next(0, 10));
        }

        return $"{timestamp}-{new string(randomDigits)}";
    }

    static string GenerateEventId()
    {
        const string characters = "abcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var firstPart = GenerateRandomString(random, characters, 8);
        var secondPart = GenerateRandomString(random, "0123456789", 4);
        var thirdPart = GenerateRandomString(random, characters, 4);
        var fourthPart = GenerateRandomString(random, characters, 4);
        var fifthPart = GenerateRandomString(random, characters, 12);

        return $"{firstPart}-{secondPart}-{thirdPart}-{fourthPart}-{fifthPart}";
    }

    static string GenerateRandomString(Random random, string chars, int length)
    {
        var stringChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }
        return new string(stringChars);
    }

    private static readonly string filePath = "keys.txt"; // Замените на путь к вашему файлу
    private static readonly Mutex mutex = new Mutex(); // Создание мьютекса для синхронизации
    
    static void StorePromoCode(string promoCode)
    {
        try
        {
            mutex.WaitOne(); // Захват мьютекса

            // Открытие файла и запись строки
            using (var fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
            using (var streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.WriteLine(promoCode);
            }

            Console.WriteLine(Thread.CurrentThread.Name + ": " +$"Stored promo code: {promoCode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(Thread.CurrentThread.Name + ": " +$"Ошибка при записи в файл: {ex.Message}");
        }
        finally
        {
            mutex.ReleaseMutex(); // Освобождение мьютекса
        }
    }
}



