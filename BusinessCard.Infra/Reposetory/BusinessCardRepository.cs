using BusinessCard.core.Data;
using BusinessCard.core.DTO.BusinessCards;
using BusinessCard.core.IReposetory;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CsvHelper;
using static Azure.Core.HttpHeader;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BusinessCard.Infra.Reposetory
{
    public class BusinessCardRepository : GenericRepository<Businesscards> , IBusinessCardRepository
    {
        private readonly BusinessCardDbContext _context;


        public BusinessCardRepository(BusinessCardDbContext context):base(context)
        {
        
        _context = context;
        
        }

         public async Task<IEnumerable<Businesscards>> FilterAsync(string name, DateTime? dob, string phone, string gender, string email)
        {
            var query = _context.Set<Businesscards>().AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(bc => bc.Name.Contains(name));
            }

            if (dob.HasValue)
            {
                query = query.Where(bc => bc.DateOfBirth == dob.Value);
            }

            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(bc => bc.Phone.Contains(phone));
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(bc => bc.Gender == gender);
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(bc => bc.Email.Contains(email));
            }

            return await query.ToListAsync();
         }

        public async Task<byte[]> ExportToCsvAsync()
        {
            var businessCards = await GetAllAsync();
            var csv = new StringBuilder();
            csv.AppendLine("Id,Name,Phone,Email,DateOfBirth,Gender,Address,Notes,Photo,UserId,CreatedAt,UpdatedAt");

            foreach (var card in businessCards)
            {
                csv.AppendLine($"{card.Id},{card.Name},{card.Phone},{card.Email}" +
                    $",{card.DateOfBirth?.ToString("yyyy-MM-dd")}" +
                    $",{card.Gender},{card.Address}" +
                    $",{card.Notes},{card.Photo},{card.UserId},{card.CreatedAt.ToString("yyyy-MM-dd")},{card.UpdatedAt.ToString("yyyy-MM-dd")}");
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }
        public async Task<byte[]> ExportToXmlAsync()
        {
            var businessCards = await GetAllAsync();
            var xml = new XElement("BusinessCards",
                from card in businessCards
                select new XElement("BusinessCard",
                    new XElement("Id", card.Id),
                    new XElement("Name", card.Name),
                    new XElement("Phone", card.Phone),
                    new XElement("Email", card.Email),
                    new XElement("DateOfBirth", card.DateOfBirth?.ToString("yyyy-MM-dd")),
                    new XElement("Gender", card.Gender),
                    new XElement("Address", card.Address),
                    new XElement("Notes", card.Notes),
                    new XElement("Photo",card.Photo),
                    new XElement("UserId", card.UserId)


                )
            );

            var xmlString = xml.ToString();
            return Encoding.UTF8.GetBytes(xmlString);
        }

        public async Task ImportFromCsvAsync(Stream fileStream)
        {
            using (var reader = new StreamReader(fileStream))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<CreateBusinessCardDto>().ToList();
                foreach (var record in records)
                {
                    var businessCard = new Businesscards
                    {
                        Name = record.Name,
                        Phone = record.Phone,
                        Email = record.Email,
                        DateOfBirth = record.DateOfBirth,
                        Gender = record.Gender,
                        Address = record.Address,
                        Notes = record.Notes,
                        Photo = record.Photo,
                        UserId = record.UserId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt= DateTime.Now

                    };

                    await AddAsync(businessCard); // Assuming you have an AddAsync method
                }
            }
        }

        public async Task ImportFromXmlAsync(Stream fileStream)
        {
            var xmlDocument = XDocument.Load(fileStream);
            var businessCards = xmlDocument.Descendants("BusinessCard")
                .Select(x => new CreateBusinessCardDto
                {
                    Name = x.Element("Name")?.Value,
                    Phone = x.Element("Phone")?.Value,
                    Email = x.Element("Email")?.Value,
                    DateOfBirth = DateTime.TryParse(x.Element("DateOfBirth")?.Value, out var dob) ? dob : (DateTime?)null,
                    Gender = x.Element("Gender")?.Value,
                    Address = x.Element("Address")?.Value,
                    Notes = x.Element("Notes")?.Value,
                    Photo = x.Element("Photo")?.Value,
                    UserId= int.TryParse(x.Element("UserId")?.Value, out var userId) ? userId : 0

                });

            foreach (var record in businessCards)
            {
                var businessCard = new Businesscards
                {
                    Name = record.Name,
                    Phone = record.Phone,
                    Email = record.Email,
                    DateOfBirth = record.DateOfBirth,
                    Gender = record.Gender,
                    Address = record.Address,
                    Notes = record.Notes,
                    Photo = record.Photo,
                    UserId= record.UserId
                };

                await AddAsync(businessCard); // Assuming you have an AddAsync method
            }
        }






    }
}
