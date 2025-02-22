﻿using BusinessCard.core.Data;
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








    }
}
