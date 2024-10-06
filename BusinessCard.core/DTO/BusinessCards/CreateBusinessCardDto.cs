using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCard.core.DTO.BusinessCards
{
    public class CreateBusinessCardDto:BaseBusinessCardsDto
    {

        public int UserId { get; set; }
    }
}
