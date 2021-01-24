using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromotionEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTesting
{
    [TestClass]
    public class Test
    {
        List<Promotion> promotions = new List<Promotion>();

        [TestInitialize]
        public void Initilize()
        {
            // add promotions
            // SKU and QTY
            Dictionary<String, int> p1 = new Dictionary<String, int>();
            Dictionary<String, int> p2 = new Dictionary<String, int>();
            Dictionary<String, int> p3 = new Dictionary<String, int>();
            Dictionary<String, int> p4 = new Dictionary<String, int>();

            p1.Add("A", 3);
            p2.Add("B", 2);
            p3.Add("C", 1);
            p3.Add("D", 1);
            p4.Add("E", 5);

            promotions = new List<Promotion>()
            {
                new Promotion(1, p1, 130, PromotionTypes.FLATRATE),
                new Promotion(2, p2, 45, PromotionTypes.FLATRATE),
                new Promotion(3, p3, 30, PromotionTypes.FLATRATE),
                new Promotion(4, p4, 10, PromotionTypes.FLATPERCENTAGE),
            };
        }
        [TestMethod]
        public void ScenarioA()
        {
            //create orders
            List<Order> orders = new List<Order>();
            Order items1 = new Order(1, new List<Product>() { new Product("A") });
            Order items2 = new Order(2, new List<Product>() { new Product("B")});
            Order items3 = new Order(3, new List<Product>() { new Product("C")});
            orders.AddRange(new Order[] { items1, items2, items3});

            decimal expectedValue = 100M;
            decimal actualValue = PromotionChecker.GetTotalPriceOfOrder(orders, promotions);
            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod]
        public void ScenarioB()
        {
            //create orders
            List<Order> orders = new List<Order>();
            Order items1 = new Order(1, new List<Product>() { new Product("A"), new Product("A"), new Product("A"), new Product("A"), new Product("A") });
            Order items2 = new Order(2, new List<Product>() { new Product("B"), new Product("B"), new Product("B"), new Product("B"), new Product("B") });
            Order items3 = new Order(3, new List<Product>() { new Product("C") });
            orders.AddRange(new Order[] { items1, items2, items3 });
            decimal expectedValue = 370M;
            decimal actualValue = PromotionChecker.GetTotalPriceOfOrder(orders, promotions);
            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod]
        public void ScenarioC()
        {
            //create orders
            List<Order> orders = new List<Order>();
            Order items1 = new Order(1, new List<Product>() { new Product("A"), new Product("A"), new Product("A")});
            Order items2 = new Order(2, new List<Product>() { new Product("B"), new Product("B"), new Product("B"), new Product("B"), new Product("B") });
            Order items3 = new Order(3, new List<Product>() { new Product("C"), new Product("D") });
            orders.AddRange(new Order[] { items1, items2, items3 });
            decimal expectedValue = 280M;
            decimal actualValue = PromotionChecker.GetTotalPriceOfOrder(orders, promotions);
            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod]
        public void ScenarioD()
        {
            //create orders
            List<Order> orders = new List<Order>();
            Order items1 = new Order(1, new List<Product>() { new Product("A"), new Product("A"), new Product("A") });
            Order items2 = new Order(2, new List<Product>() { new Product("B"), new Product("B"), new Product("B"), new Product("B"), new Product("B") });
            Order items3 = new Order(3, new List<Product>() { new Product("E"), new Product("E") , new Product("E") , new Product("E") , new Product("E") , new Product("E") });
            orders.AddRange(new Order[] { items1, items2, items3 });
            decimal expectedValue = 378M;
            decimal actualValue = PromotionChecker.GetTotalPriceOfOrder(orders, promotions);
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
