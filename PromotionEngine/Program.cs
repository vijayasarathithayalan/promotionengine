using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }

    public class Product
    {

        public string Id { get; set; }
        public decimal Price { get; set; }


        public Product(string id)
        {
            this.Id = id;
            switch (id)
            {
                case "A":
                    this.Price = 50m;

                    break;
                case "B":
                    this.Price = 30m;

                    break;
                case "C":
                    this.Price = 20m;

                    break;
                case "D":
                    this.Price = 15m;
                    break;
                case "E":
                    this.Price = 80m;
                    break;
            }
        }
    }

    public enum PromotionTypes
    {
        FLATRATE =0,
        FLATPERCENTAGE = 1
    }
    public class Promotion
    {
        public int PromotionID { get; set; }
        public Dictionary<string, int> ProductInfo { get; set; }
        public decimal PromoPrice { get; set; }
        public PromotionTypes PromotionType { get; set; }
        public Promotion(int _promID, Dictionary<string, int> _prodInfo, decimal _pp, PromotionTypes _promotionType)
        {
            this.PromotionID = _promID;
            this.ProductInfo = _prodInfo;
            this.PromoPrice = _pp;
            this.PromotionType = _promotionType;
        }
    }

    public class Order
    {
        public int OrderID { get; set; }
        public List<Product> Products { get; set; }

        public Order(int _oid, List<Product> _prods)
        {
            this.OrderID = _oid;
            this.Products = _prods;
        }
    }

    public static class PromotionChecker
    {
        //returns PromotionID and count of promotions
        public static decimal GetTotalPriceOfItem(Order ord, Promotion prom, ref int remainingQty)
        {
            decimal d = 0M;
            //get count of promoted products in order
            var copp = ord.Products
                .GroupBy(x => x.Id)
                .Where(grp => prom.ProductInfo.Any(y => grp.Key == y.Key && grp.Count() >= y.Value))
                .Select(grp => grp.Count())
                .Sum();
            //get count of promoted products from promotion
            int ppc = prom.ProductInfo.Sum(kvp => kvp.Value);
            while (copp >= ppc)
            {
                d += prom.PromoPrice;
                copp -= ppc;
            }
            if(copp>0)
                remainingQty += copp;
            return d;
        }

        public static decimal GetTotalPriceOfOrder(List<Order> orders, List<Promotion> promotions)
        {
            decimal cartTotal = 0M;
            foreach (Order ord in orders)
            {
                int remainingQty = 0;
                List<decimal> promoprices = promotions
                    .Select(promo => PromotionChecker.GetTotalPriceOfItem(ord, promo, ref remainingQty))
                    .ToList();
                decimal origprice = ord.Products.Sum(x => x.Price);
                decimal priceQty = ord.Products.Select(q => q.Price).FirstOrDefault();

                decimal promoprice = promoprices.Sum();
                PromotionTypes promoType = promotions.Where(x => x.ProductInfo.Select(z=>z.Key).FirstOrDefault().Equals(ord.Products.FirstOrDefault().Id)).Select(p => p.PromotionType).FirstOrDefault();
                decimal finalPrice = 0m;
                if (promoType == PromotionTypes.FLATPERCENTAGE)
                {
                    finalPrice = origprice * (promoprice / 100);
                }
                else if (promoType == PromotionTypes.FLATRATE)
                {
                    finalPrice = promoprice;
                }
                cartTotal += finalPrice==0M? origprice:finalPrice + (remainingQty * priceQty);
            }
            return cartTotal;
        }
    }
}
