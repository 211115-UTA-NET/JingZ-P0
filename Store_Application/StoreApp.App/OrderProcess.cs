using StoreApp.DataInfrastructure;
using StoreApp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.App
{
    public class OrderProcess
    {
        private readonly IRepository _repository;
        public OrderProcess(IRepository repository)
        {
            _repository = repository;
        }

        public string DisplayOrderDetail(int customerID, List<string> productNames, List<int> productQty, int locationID, out bool Processfailed)
        {
            // Checking List contents:
            //for (int i = 0; i < productNames.Count; i++)
            //{
            //    Console.WriteLine(productNames[i] + " | " + productQty[i]);
            //}
            var receipt = new StringBuilder();
            int orderNumber = _repository.GetOrderNumber(customerID);
            List<Order> order = new();
            if (orderNumber < 0)
            {
                Console.WriteLine("--- For some reason, unable to process your order. ---");
                Processfailed = true;
            }
            else
            {
                // Console.WriteLine("Order #: "  + orderNumber);
                for (int i = 0; i < productNames.Count; i++)
                {
                    // price does not matters here
                    order.Add(new(orderNumber, productNames[i], productQty[i], locationID, null));
                }
                IEnumerable<Order> allRecords = ProcessOrder(order);
                if (allRecords == null || !allRecords.Any())
                {
                    receipt.AppendLine("--- Your Input is invalid, please try again. ---");
                    Processfailed = true;
                }
                else
                {
                    List<decimal> price = _repository.GetPrice(order);
                    int once = 0;
                    int i = 0;
                    decimal totalPrice = 0;
                    foreach (var record in allRecords)
                    {
                        if(once == 0)
                        {
                            receipt.AppendLine(string.Format("Order#: {0} | Ordered at: {1,10}", record.OrderNum, record.OrderDate));
                            receipt.AppendLine($"Product Name\t\tPurchased Amount\t\tPrice");
                            receipt.AppendLine("---------------------------------------------------------------");
                            once++;
                        }
                        price[i] *= record.ProductQty;
                        try
                        {
                            receipt.AppendLine(string.Format("{0,30} | {1,10} | {2,10}", record.ProductName, record.ProductQty, price[i]));
                        } catch(IndexOutOfRangeException e)
                        {
                            Console.WriteLine(e.StackTrace);
                        }
                        totalPrice += price[i];
                        i++;
                    }
                    receipt.AppendLine("---------------------------------------------------------------");
                    receipt.AppendLine($"Total Price: ${totalPrice}\n");
                    Processfailed = false;
                }
            }
            return receipt.ToString();
        }

        private IEnumerable<Order> ProcessOrder(List<Order> order)
        {
            /*
             * Ex:
             * INSERT OrderProduct (OrderNum, ProductName, Amount, LocationID) VALUES (2,'Masking Tape', 5, 3);
             */
            IEnumerable<Order> allRecords = _repository.AddOrder(order);
            return allRecords;
        }
    }
}
