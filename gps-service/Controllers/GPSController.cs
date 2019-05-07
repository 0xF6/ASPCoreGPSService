namespace gps_service.Controllers
{
    using System;
    using System.Linq;
    using Core;
    using Core.HttpV2;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class GPSController : ControllerBase
    {
        private readonly DBContext Ctx;
        public GPSController(DBContext ctx) => this.Ctx = ctx;

        [HttpPost]
        public IActionResult Store([FromBody] GPSModel[] model)
        {
            if (model.Length % 2 != 0)
                return BadRequest(new {message = $"geo len is not % 2." });
            var sessionID = Guid.NewGuid().ToString();

            Ctx.Positions.AddRange(model.Select(x => new GeoPosition(x.Latitude, x.Longitude, sessionID)));

            var entityApplied = Ctx.SaveChanges();
            if (entityApplied > 0)
                return Ok(new {sessionID, message = $"Entity applied in database: {entityApplied}"});
            return StatusCode(500, new {message = $"Failed applied entity to database."});
        }
        [HttpView]
        public IActionResult BySession([FromQuery] string sid)
        {
            if (string.IsNullOrEmpty(sid))
                return BadRequest(new {message = "Invalid sid."});
            if (StorageManager.AllSessions.All(x => x != sid))
                return StatusCode(404, new {message = "Sid not found."});
            var storage = StorageManager.AllStorages.First(x => x.SessionID == sid);
            return Ok(new { message = "found", result = storage });
        }

    }
}