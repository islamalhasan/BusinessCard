using BusinessCard.core.IReposetory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCard.core.IServieces
{
    public interface IBusinessCardServiece : IBusinessCardRepository
    {

        Task<byte[]> ExportToCsvAsync();
        Task<byte[]> ExportToXmlAsync();
        Task ImportFromCsvAsync(Stream fileStream);
        Task ImportFromXmlAsync(Stream fileStream);


    }
}
