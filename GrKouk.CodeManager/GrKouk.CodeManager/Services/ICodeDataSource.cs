using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GrKouk.CodeManager.Models;

namespace GrKouk.CodeManager.Services
{
    public interface ICodeDataSource
    {
        Task<IEnumerable<CodeDto>> GetCodesAsync(string codeBase);
    }
}
