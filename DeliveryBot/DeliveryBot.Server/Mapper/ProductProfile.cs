using AutoMapper;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Product;
using DeliveryBot.Shared.Dto.Product;

namespace DeliveryBot.Server.Mapper;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductCommand, Product>();
        CreateMap<EditProductCommand, Product>();
        CreateMap<Product, CompanyProductInfoDto>();
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();
    }
}