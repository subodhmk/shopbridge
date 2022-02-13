using AutoMapper;
using Shopbridge_base.Domain.Models;
using Shopbridge_base.Domain.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_base.ShopBridgeMapper
{
    public class ShopBridgeMapping : Profile
    {
        public ShopBridgeMapping()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
