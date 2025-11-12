using iths_labb_3_quiz_configurator.Models;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace iths_labb_3_quiz_configurator.Services;

public interface IApiService
{
    Task<ObservableCollection<T>> GetAsync<T>(
        HttpClient httpClient,
        string urlEnd,
        Func<string, ObservableCollection<T>> jsonFactory);

    Task<HttpResponseMessage> ApiCall(
        HttpClient httpClient,
        string urlEnd);

    string Decode(string s);

    ObservableCollection<Category> CategoriesFromJson(string json);

    ObservableCollection<Question> QuestionsFromJson(string json);
}

