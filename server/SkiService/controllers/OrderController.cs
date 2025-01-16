using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkiService.Models;



namespace SkiService.Controllers
{
    //[Route("[controller]")]
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    [AllowAnonymous]
    public class OrderController : ControllerBase
    {
        private readonly SkiServiceDbContext _context;

        public OrderController(SkiServiceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders = _context.Orders.Select(o => new
            {
                o.Id,
                o.CustomerName,
                o.Email,
                o.Phone,
                o.Priority,
                o.Status,
                o.ServiceId, // Stelle sicher, dass diese Spalte existiert
            }).ToList();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order cannot be null.");
            }

            // Überprüfe die ServiceId
            if (!_context.Service.Any(s => s.Id == order.ServiceId))
            {
                return BadRequest("Invalid ServiceId.");
            }

            // Überprüfe, ob das PickupDate im Request enthalten ist
            if (order.pickup_date == default)
            {
                return BadRequest("PickupDate is required.");
            }

            // Order speichern
            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            if (!_context.Users.Any(u => u.Username == User.Identity.Name)) 
            {
                return Unauthorized("Unauthorized Access");
            }

             if (!_context.Userroles.Any(ur => ur.User.Username == User.Identity.Name && (ur.Role == "Mitarbeiter" || ur.Role == "Admin")))
            {
                return Unauthorized("You do not have permission to update orders.");
            }

            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            order.CustomerName = updatedOrder.CustomerName;
            order.Email = updatedOrder.Email;
            order.Phone = updatedOrder.Phone;
            order.Priority = updatedOrder.Priority;
            order.pickup_date = updatedOrder.pickup_date;
            order.ServiceId = updatedOrder.ServiceId;

            _context.SaveChanges();

            return Ok(order);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            if (!_context.Users.Any(u => u.Username == User.Identity.Name)) 
            {
                return Unauthorized("Unauthorized Access");
            }

            if (!_context.Userroles.Any(ur => ur.User.Username == User.Identity.Name && (ur.Role == "Mitarbeiter" || ur.Role == "Admin")))
            {
                return Unauthorized("You do not have permission to delete orders.");
            }

            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return Ok(order);
}

        [HttpOptions("Order")]
        public IActionResult Options()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            Response.Headers.Add("Access-Control-Allow-Credentials", "true");

            return Ok();
        }
    }
}
