using BusinessCard.core.Data;
using BusinessCard.core.IServieces;
using BusinessCard.Infra.Reposetory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCard.Infra.Servieces
{
    public class BusinesscardServiece: BusinessCardRepository , IBusinessCardServiece
    {
        public BusinesscardServiece(BusinessCardDbContext context):base(context)
        {

        }


    }
}
