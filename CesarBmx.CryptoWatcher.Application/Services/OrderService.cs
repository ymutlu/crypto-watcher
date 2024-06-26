﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.Shared.Application.Exceptions;
using CesarBmx.Shared.Logging.Extensions;
using CesarBmx.CryptoWatcher.Domain.Expressions;
using CesarBmx.CryptoWatcher.Application.Messages;
using CesarBmx.CryptoWatcher.Domain.Models;
using CesarBmx.CryptoWatcher.Domain.Builders;
using CesarBmx.CryptoWatcher.Domain.Types;
using CesarBmx.CryptoWatcher.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CesarBmx.CryptoWatcher.Application.Services
{
    public class OrderService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            MainDbContext mainDbContext,
            IMapper mapper,
            ILogger<OrderService> logger)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<Responses.Order>> GetUserOrders(string userId)
        {
            // Get user
            var user = await _mainDbContext.Users.FindAsync(userId);

            // Check if it exists
            if (user == null) throw new NotFoundException(UserMessage.UserNotFound);

            // Get all orders
            var orders = await _mainDbContext.Orders.Where(x=>x.UserId == userId).ToListAsync();

            // Response
            var response = _mapper.Map<List<Responses.Order>>(orders);

            // Return
            return response;
        }
        public async Task<Responses.Order> GetOrder(Guid orderId)
        {
            // Get order
            var order = await _mainDbContext.Orders.FindAsync(orderId);

            // Throw NotFound if it does not exist
            if (order == null) throw new NotFoundException(OrderMessage.OrderNotFound);

            // Response
            var response = _mapper.Map<Responses.Order>(order);

            // Return
            return response;
        }

        public async Task<List<Order>> AddOrders(List<Watcher> watchers)
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Grab watchers willing to buy or sell
            watchers = watchers.Where(WatcherExpression.WatcherBuyingOrSelling().Compile()).ToList();

            // Build new orders
            var newOrders = OrderBuilder.BuildNewOrders(watchers);

            // Add
            _mainDbContext.Orders.AddRange(newOrders);

            // Save
            await _mainDbContext.SaveChangesAsync();

            // Stop watch
            stopwatch.Stop();

            // Log into Splunk
            _logger.LogSplunkInformation(nameof(AddOrders), new
            {
                newOrders.Count,
                ExecutionTime = stopwatch.Elapsed.TotalSeconds
            });

            // Return
            return newOrders;
        }
        public async Task<List<Order>> ProcessOrders(List<Order> orders, List<Watcher> watchers)
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Grab orders ready to buy or sell
            foreach (var order in orders)
            {
                // Mark as filled
                order.MarkAsFilled();

                // Update order
                _mainDbContext.Orders.Update(order);
                
                // Get watcher
                var watcher = watchers.FirstOrDefault(x => x.WatcherId == order.WatcherId);

                // Make sure watcher exists
                if (watcher == null) throw new ApplicationException("Watcher is expected");

                switch (order.OrderType)
                {
                    case  OrderType.BUY:
                        watcher.SetAsBought(); // Set as bought
                        break;
                    case OrderType.SELL:
                        watcher.SetAsSold(); // Set as sold
                        break;
                    default:
                        throw new NotImplementedException();
                }
                
                // Update watcher
                _mainDbContext.Watchers.Update(watcher);
            }

            // Save
            await _mainDbContext.SaveChangesAsync();

            // Stop watch
            stopwatch.Stop();

            // Log into Splunk
            _logger.LogSplunkInformation(nameof(AddOrders), new
            {
                orders.Count,
                ExecutionTime = stopwatch.Elapsed.TotalSeconds
            });

            // Return
            return orders;
        }
    }
}
