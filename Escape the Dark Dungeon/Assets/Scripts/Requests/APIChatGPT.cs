using UnityEngine;
using System;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleJSON;

public class APIChatGPT : MonoBehaviour
{
    private const string APIKey = "";
    private const string ChatGPTURL = "https://api.openai.com/v1/completions";

    public async Task CompleteDialogue(string dialoguePrompt, Action<string> onComplete)
    {
        Debug.Log("CompleteDialogue method called");

        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", APIKey);
                Debug.Log("HTTP Client setup complete");

                var requestData = new
                {
                    model = "gpt-3.5-turbo-instruct",
                    prompt = dialoguePrompt,
                    max_tokens = 300,
                    temperature = 0.7
                };

                var requestContent = new StringContent(JsonConvert.SerializeObject(requestData));
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                Debug.Log("Request content prepared");

                HttpResponseMessage response = await client.PostAsync(ChatGPTURL, requestContent);
                Debug.Log("HTTP POST request sent");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Debug.Log("Response received: " + responseContent);
                    JSONNode responseJson = JSON.Parse(responseContent);
                    string completedDialogue = responseJson["choices"][0]["text"].Value;
                    Debug.Log("Dialogue completed: " + completedDialogue);
                    onComplete(completedDialogue);
                }
                else
                {
                    Debug.LogError("Error completing dialogue: " + response.ReasonPhrase);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Exception caught: " + e.Message);
        }
    }
}
