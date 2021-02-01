﻿using System;
using System.Collections.Generic;
using CryptoWatcher.Application.Responses;

namespace CryptoWatcher.Application.FakeResponses
{
    public static class FakeLine
    {
        public static Line GetFake_Bitcoin_Price()
        {
            return new Line
            {
                Time = DateTime.Now.AddHours(-1),
                UserId = "master",
                CurrencyId = "bitcoin",
                IndicatorId = "price",
                Value = 1.5m,
                AverageBuy = 15,
                AverageSell = 8
            };
        }
        public static Line GetFake_Bitcoin_RSI()
        {
            return new Line
            {
                Time = DateTime.Now,
                UserId = "master",
                CurrencyId = "bitcoin",
                IndicatorId = "rsi",
                Value = 1.5m,
                AverageBuy = 9,
                AverageSell = 6
            };
        }
        public static List<Line> GetFake_List()
        {
            return new List<Line>
            {
                GetFake_Bitcoin_Price(),
                GetFake_Bitcoin_RSI()
            };
        }
    }
}
