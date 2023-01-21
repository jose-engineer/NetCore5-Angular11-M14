using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;

namespace DutchTreat.Data
{
  public class DutchMappingProfile : Profile
  {
    public DutchMappingProfile()
    {
      CreateMap<Order, OrderViewModel>()
        .ForMember(o => o.OrderId, ex => ex.MapFrom(i => i.Id))
        .ReverseMap(); //take the previous map on line 17 and create another map in the opposite order

      CreateMap<OrderItem, OrderItemViewModel>()
        .ReverseMap() //Going from OrderItem to OrderItemViewModel
        .ForMember(m => m.Product, opt => opt.Ignore()); //we are saying not to map when we go from OrderItemViewModel to OrderItem the Product to the ProductId. Since OrderItemViewModel already has a ProdcutId on it will naturally map it back to our ProdcutId so that the FK still continue to work, we are just including the product in the OrderItem so we can refer to it in the client.
        }
  }
}
