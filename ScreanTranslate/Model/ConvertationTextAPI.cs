using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ScreanTranslate.Model
{
    public class ConvertationTextAPI
    {
        public static string Base64 {  get; set; }
        public static string? Text { get; set; }

        private static readonly string URL = "https://ocr.api.cloud.yandex.net/ocr/v1/recognizeText";
        private static readonly string Key = ExtractAPIKey.Key();

        public static async Task<string> Vision()
        {
            Base64 = CopyScreen.Base64;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Api-Key {Key}");

                    var body = new
                    {
                        mimeType = "JPEG",
                        languageCodes = new string[] { "*" },
                        model = "page",
                        content = Base64
                    };

                    string json = System.Text.Json.JsonSerializer.Serialize(body);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(URL, content).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();

                        var jso = JsonDocument.Parse(data).RootElement;
                        Text = jso.GetProperty("result").GetProperty("textAnnotation").GetProperty("fullText").GetString();

                        return Text;
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
