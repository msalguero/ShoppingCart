using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Data.Entities;
using ShoppingCart.Data.Repositories;

namespace ShoppingCart
{
    public class InventoryManager
    {
        private readonly IStockMovementsRepository _stockRepository;

        public InventoryManager(IStockMovementsRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public List<Report> GetReport()
        {
            var list = new List<Report>();
            var movements = _stockRepository.GetMovements();
            foreach (var move in movements)
            {
                AddMove(move, list);
            }
            return list;
        }

        private void AddMove(StockMovements move, List<Report> list)
        {
            foreach (var report in list)
            {
                if (report.ProductId == move.ProductId)
                {
                    report.Quantity += move.Quantity;
                    return;
                }
            }
            var newReport = new Report();
            newReport.ProductId = move.ProductId;
            newReport.Quantity = move.Quantity;

            list.Add(newReport);
        }
    }

    public class Report
    {
        public int ProductId;
        public int Quantity;
    }
}
