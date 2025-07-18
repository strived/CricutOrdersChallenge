﻿using Cricut.Orders.Api.Mappings;
using Cricut.Orders.Api.ViewModels;
using Cricut.Orders.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Cricut.Orders.Api.Controllers
{
    [Route("v1/orders")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IOrderDomain _orderDomain;

        public OrdersController(IOrderDomain orderDomain)
        {
            _orderDomain = orderDomain;
        }

        [HttpPost]
        public async Task<ActionResult<OrderViewModel>> CreateNewOrderAsync([FromBody] NewOrderViewModel newOrderVM)
        {
            var newOrder = newOrderVM.ToDomainModel();
            var savedOrder = await _orderDomain.CreateNewOrderAsync(newOrder);
            return Ok(savedOrder.ToViewModel());
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetCustomerOrdersAsync(int customerId)
        {           
            var Orders = await _orderDomain.GetAllOrdersForCustomerAsync(customerId);
            if (Orders == null || Orders.Count() <= 1)
            {
                return BadRequest("Customer id not found.");

            }
            else
            {
                return Ok(Orders);
            }
        }
    }
}
