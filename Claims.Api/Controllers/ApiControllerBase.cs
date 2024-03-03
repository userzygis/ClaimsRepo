using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers
{
    public abstract class ApiControllerBase: ControllerBase
    {
        [NonAction]
        protected ObjectResult CoverNotFound(string Id)
        {
            return NotFound($"Cover '{Id}' not found");
        }

        [NonAction]
        protected ObjectResult ClaimNotFound(string Id)
        {
            return NotFound($"Claim '{Id}' not found");
        }

        [NonAction]
        private ObjectResult NotFound(string message)
        {
            return StatusCode(StatusCodes.Status404NotFound, message);
        }
    }
}
