using iths_labb_3_quiz_configurator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iths_labb_3_quiz_configurator.Services;

public interface IDataService
{
    Task<IList<QuestionPack>> LoadPacksAsync();
    Task SavePacksAsync(IEnumerable<QuestionPack> pack);
}
