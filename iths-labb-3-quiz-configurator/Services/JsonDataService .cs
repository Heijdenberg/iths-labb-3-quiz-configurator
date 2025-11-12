using iths_labb_3_quiz_configurator.Models;
using System.IO;
using System.Text.Json;

namespace iths_labb_3_quiz_configurator.Services;

internal class JsonDataService : IDataService
{
    private readonly string _directoryPath;
    private readonly string _filePath;

    public JsonDataService()
    {
        _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QuizConfigurator");
        _filePath = Path.Combine(_directoryPath, "packs.json");
    }
    public async Task<IList<QuestionPack>> LoadPacksAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<QuestionPack>();
        }

        try
        {
            string json = await File.ReadAllTextAsync(_filePath);
            var packs = JsonSerializer.Deserialize<List<QuestionPack>>(json) ?? new List<QuestionPack>();
            return packs;
        }
        catch
        {
            return new List<QuestionPack>();
        }
    }

    public async Task SavePacksAsync(IEnumerable<QuestionPack> packs)
    {
        Directory.CreateDirectory(_directoryPath);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        string json = JsonSerializer.Serialize(packs, options);
        await File.WriteAllTextAsync(_filePath, json);
    }
}
