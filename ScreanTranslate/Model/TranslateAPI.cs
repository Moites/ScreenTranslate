using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ScreanTranslate.Model
{
    class TranslateAPI
    {
        public static string? TranslateText { get; set; }
        private static readonly string URL = "https://translate.api.cloud.yandex.net/translate/v2/translate";
        private static readonly string Key = ExtractAPIKey.Key();

        public static async Task<string> Translate(string text)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Api-Key {Key}");

                    var requestData = new
                    {
                        texts = new[] { text },
                        targetLanguageCode = "ru"
                    };

                    string json = System.Text.Json.JsonSerializer.Serialize(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(URL, content).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        string body = await response.Content.ReadAsStringAsync();

                        var jso = JsonDocument.Parse(body).RootElement;
                        var translations = jso.GetProperty("translations");
                        TranslateText = translations[0].GetProperty("text").GetString();

                        return TranslateText;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
