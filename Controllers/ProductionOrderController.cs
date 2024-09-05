using Microsoft.AspNetCore.Mvc;
using ChallengeNeo.Models;
using System;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;

namespace ChallengeNeo.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductionOrderController : ControllerBase
	{
		[HttpGet]
		public IActionResult GetProductionOrder()
		{
            string filePath = "data/productionOrder.json";

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
		public IActionResult PostProductionOrder([FromBody] ProductionOrder ProductionOrder)
		{
			if (ProductionOrder == null)
			{
				return BadRequest();
			}
			string filePath = "data/productionOrder.json";

			List<ProductionOrder> productionOrders;

			if (System.IO.File.Exists(filePath))
			{
				string jsonString = System.IO.File.ReadAllText(filePath);

				productionOrders = JsonConvert.DeserializeObject<List<ProductionOrder>>(jsonString) ?? new List<ProductionOrder>();
			}
			else
			{
				productionOrders = new List<ProductionOrder>();
			}

			productionOrders.Add(ProductionOrder);

			string updatedJson = JsonConvert.SerializeObject(productionOrders, Formatting.Indented);

			System.IO.File.WriteAllText(filePath, updatedJson);

			var response =
				new
				{
					ProductionOrder = ProductionOrder,
					message = "Object created!"
				};

			return Ok(response);
		}

		[HttpDelete]
		public IActionResult DeleteProductionOrder()
		{
			string filePath = "data/productionOrder.json";

			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.WriteAllText(filePath, string.Empty);
				return Ok();
			}
			else
			{
				return NotFound(new { message = "File not found." });
			}

		}
	}
}
