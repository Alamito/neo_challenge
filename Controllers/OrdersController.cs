using Microsoft.AspNetCore.Mvc;
using ChallengeNeo.Models;
using System;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using ClosedXML.Excel;

namespace ChallengeNeo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetOrders()
        {
            string filePath = "data/Orders.json";

            List<ProductionOrder> productionOrders;

            if (System.IO.File.Exists(filePath))
            {
                string jsonString = System.IO.File.ReadAllText(filePath);

                productionOrders = JsonConvert.DeserializeObject<List<ProductionOrder>>(jsonString) ?? new List<ProductionOrder>(); ;
            }
            else
            {
                productionOrders = new List<ProductionOrder>();
            }

            string productionOrdersJSON = JsonConvert.SerializeObject(productionOrders, Formatting.Indented);

            return Ok(productionOrdersJSON);
        }

        [HttpPost]
        public IActionResult PostOrders()
        {
            string excelFilePath = "tmp/Orders.xlsx";
            
            using var workbook = new XLWorkbook(excelFilePath);

            var ws = workbook.Worksheet(1);
            var rows = ws.LastRowUsed().RowNumber();
            var columnCount = ws.LastColumnUsed().ColumnNumber();

            List<string> data = new List<string>();

            for (int i = 0; i < columnCount; i++)
            {
                data.Add(ws.Cell(1, i + 1).Value.ToString());
            }

            List<ProductionOrder> productionOrders = new List<ProductionOrder>();
            int countOrders = 0;

            for (int i = 2; i <= rows; i++)
            {
                productionOrders.Add(new ProductionOrder
                {
                    OrderId = i - 1,
                    OrderNumber = int.Parse(ws.Cell(i, 1).Value.ToString()),
                    OperationNumber = int.Parse(ws.Cell(i, 2).Value.ToString()),
                    Quantity = float.Parse(ws.Cell(i, 3).Value.ToString()),
                    DueDate = DateTime.Parse(ws.Cell(i, 4).Value.ToString()),
                    ProductNumber = int.Parse(ws.Cell(i, 5).Value.ToString()),
                    Product = ws.Cell(i, 6).Value.ToString()
                });
                countOrders++;
            }

            string filePath = "data/Orders.json";

            string ordersJSON = JsonConvert.SerializeObject(productionOrders, Formatting.Indented);

            System.IO.File.WriteAllText(filePath, ordersJSON);

            var response = new {
                message = "Orders imported!",
                total = countOrders
            };
            return Ok(response);
        }

        [HttpDelete]
        public IActionResult DeleteOrders()
        {
            string filePath = "data/Orders.json";

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.WriteAllText(filePath, string.Empty);
                return Ok(new { message = "File content cleared successfully." });
            }
            else
            {
                return NotFound(new { message = "File not found." });
            }
        }
    }
}
