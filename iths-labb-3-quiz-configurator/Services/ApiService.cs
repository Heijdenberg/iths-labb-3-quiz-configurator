using iths_labb_3_quiz_configurator.Models;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace iths_labb_3_quiz_configurator.Services;

public class ApiService : IApiService
{
    public async Task<ObservableCollection<T>> GetAsync<T>(HttpClient httpClient, string urlEnd, Func<string, ObservableCollection<T>> jsonFactory)
    {
        HttpResponseMessage response = await ApiCall(httpClient, urlEnd);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        ObservableCollection<T> strings = jsonFactory(jsonResponse);
        return strings;
    }

    public async Task<HttpResponseMessage> ApiCall(HttpClient httpClient, string urlEnd)
    {
        HttpResponseMessage response = await httpClient.GetAsync(urlEnd);
        response.EnsureSuccessStatusCode();

        return response;
    }
    public string Decode(string s) => WebUtility.HtmlDecode(s);
    public ObservableCollection<Category> CategoriesFromJson(string json)
    {
        ObservableCollection<string> strings = new();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        TriviaResponse? data = JsonSerializer.Deserialize<TriviaResponse>(json, options);

        foreach (var cat in data.Trivia_Categories)
        {
            strings.Add($"{cat.Id}: {Decode(cat.Name)}");
        }
        return data.Trivia_Categories;
    }
    public ObservableCollection<Question> QuestionsFromJson(string json)
    {
        ObservableCollection<Question> questions = new();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        Results? data = JsonSerializer.Deserialize<Results>(json, options);

        if (data?.results != null)
        {
            foreach (var q in data.results)
            {
                questions.Add(new Question(Decode(q.Question), Decode(q.Correct_answer), Decode(q.Incorrect_answers[0]), Decode(q.Incorrect_answers[1]), Decode(q.Incorrect_answers[2])));
            }
        }

        return questions;
    }
}