using BusinessCard.core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCard.core.IReposetory
{
    public interface IBusinessCardRepository : IGenericRepository<Businesscards>
    {
        Task<IEnumerable<Businesscards>> FilterAsync(string name, DateTime? dob, string phone, string gender, string email);
        Task<byte[]> ExportToCsvAsync();
        Task<byte[]> ExportToXmlAsync();
        Task ImportFromCsvAsync(Stream fileStream);
        Task ImportFromXmlAsync(Stream fileStream);

    }
}
