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
    public class AppointmentsController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetAppointment(int id)
        {
            string filePath = "data/appointments.json";

            List<Appointment> appointments;

            if (System.IO.File.Exists(filePath))
            {
                string jsonString = System.IO.File.ReadAllText(filePath);

                appointments = JsonConvert.DeserializeObject<List<Appointment>>(jsonString) ?? new List<Appointment>();
            }
            else
            {
                appointments = new List<Appointment>();
            }

            string appointmentsJSON = JsonConvert.SerializeObject(appointments, Formatting.Indented);

            foreach (var a in appointments)
            {
                if (a.OrderId == id)
                {
                    return Ok(a);
                }
            }

            var response = new
            {
                message = "Order ID not found"
            };
            return NotFound(response);
        }

        [HttpPost]
        public IActionResult PostAppointment()
        {
            string appointmentPath = "tmp/Apontamento.xlsx";
            string orderPath = "tmp/Orders.xlsx";

            using var workbookAppointment = new XLWorkbook(appointmentPath);
            using var workbookOrder = new XLWorkbook(orderPath);

            var wsAppointment = workbookAppointment.Worksheet(1);
            var wsOrder = workbookOrder.Worksheet(1);

            var rowsAppointment = wsAppointment.LastRowUsed().RowNumber();
            var rowsOrder = wsOrder.LastRowUsed().RowNumber();

            int countDeleted = 0;
            int appointmentFault = 0;
            List<Appointment> Appointments = new List<Appointment>();
            int id = 1;

            for (int i = 2; i <= rowsAppointment; i++)
            {
                Console.WriteLine("i: " + i);
                bool existOrder = false;
                int orderNumberAppointment = int.Parse(wsAppointment.Cell(i, 1).Value.ToString());

                for (int j = 2; j <= rowsOrder; j++)
                {
                    int orderNumberOrder = int.Parse(wsOrder.Cell(j, 1).Value.ToString());

                    if (orderNumberAppointment == orderNumberOrder)
                    {
                        existOrder = true;
                        float quantityAppointment = float.Parse(wsAppointment.Cell(i, 3).Value.ToString());
                        float quantityOrder = float.Parse(wsOrder.Cell(j, 3).Value.ToString());

                        if (quantityAppointment >= quantityOrder)
                        {
                            countDeleted++;
                        }
                        else
                        {
                            string dateString = wsAppointment.Cell(i, 4).Value.ToString();
                            DateTime date;
                            bool isValidDate = DateTime.TryParse(dateString, out date);

                            if (isValidDate)
                            {
                                Appointments.Add(new Appointment
                                {
                                    OrderId = id,
                                    OrderNumber = int.Parse(wsAppointment.Cell(i, 1).Value.ToString()),
                                    OperationNumber = int.Parse(wsAppointment.Cell(i, 2).Value.ToString()),
                                    Quantity = float.Parse(wsOrder.Cell(j, 3).Value.ToString()),
                                    ProductionDateTime = DateTime.Parse(wsAppointment.Cell(i, 4).Value.ToString())
                                });
                                id++;
                            }
                        }
                    }
                }

                if (!existOrder)
                {
                    appointmentFault++;
                }
                existOrder = false;
            }

            string filePath = "data/appointments.json";
            string appointmentsJSON = JsonConvert.SerializeObject(Appointments, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, appointmentsJSON);

            var response = new
            {
                message = "Appointments imported!",
                total = Appointments.Count,
                appointmentsDeleted = countDeleted,
                appointmentsFault = appointmentFault
            };

            return Ok(response);
        }

        [HttpDelete]
        public IActionResult DeleteAppointment()
        {
            string filePath = "data/appointments.json";

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
