using UnityEngine;
using System;
using System.Collections;
using System.Text;
using UnityEngine.Networking;

public class APIManager<T>
{
    private readonly string baseUrl = "http://localhost:7003/api/fruitNinja/";

   

    // Method to send a GET request
    public IEnumerator Get(string endpoint, Action<T> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl + endpoint))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                T responseObject = JsonUtility.FromJson<T>(jsonResponse);
                onSuccess?.Invoke(responseObject);
            }
            else
            {
                onError?.Invoke(request.error);
            }
        }
    }

    // Coroutine for sending a POST request
    private IEnumerator PostCoroutine(string endpoint, string jsonData, Action<T> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = new UnityWebRequest(baseUrl + endpoint, "POST"))
        {
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(byteData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            HandleResponse(request, onSuccess, onError);
        }
    }

    // Method to send a POST request
    public void Post(string endpoint, object data, Action<T> onSuccess, Action<string> onError)
    {
        string jsonData = JsonUtility.ToJson(data);
        CoroutineHelper.Instance.StartCoroutine(PostCoroutine(endpoint, jsonData, onSuccess, onError));
    }

    // Coroutine for sending a PUT request
    private IEnumerator PutCoroutine(string endpoint, string jsonData, Action<T> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = new UnityWebRequest(baseUrl + endpoint, "PUT"))
        {
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(byteData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            HandleResponse(request, onSuccess, onError);
        }
    }

    // Method to send a PUT request
    public void Put(string endpoint, string jsonData, Action<T> onSuccess, Action<string> onError)
    {
        CoroutineHelper.Instance.StartCoroutine(PutCoroutine(endpoint, jsonData, onSuccess, onError));
    }

    // Coroutine for sending a DELETE request
    private IEnumerator DeleteCoroutine(string endpoint, Action<string> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = UnityWebRequest.Delete(baseUrl + endpoint))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke("Deleted successfully");
            }
            else
            {
                onError?.Invoke(request.error);
            }
        }
    }

    // Method to send a DELETE request
    public void Delete(string endpoint, Action<string> onSuccess, Action<string> onError)
    {
        CoroutineHelper.Instance.StartCoroutine(DeleteCoroutine(endpoint, onSuccess, onError));
    }

    // Common method to handle responses for POST, PUT, and DELETE
    private void HandleResponse(UnityWebRequest request, Action<T> onSuccess, Action<string> onError)
    {
        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            T responseObject = JsonUtility.FromJson<T>(jsonResponse);
            onSuccess?.Invoke(responseObject);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }
}

public static class EndpointConstants
{
    public const string Session = "session";
    public const string GameStart = "gameStart";
    public const string Score = "score";
    public const string MatchEnd = "matchend";
    public const string SessionEnd = "sessionend";
}