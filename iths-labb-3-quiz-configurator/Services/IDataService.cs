using iths_labb_3_quiz_configurator.Models;

namespace iths_labb_3_quiz_configurator.Services;

public interface IDataService
{
    Task<IList<QuestionPack>> LoadPacksAsync();
    Task SavePacksAsync(IEnumerable<QuestionPack> pack);
}
