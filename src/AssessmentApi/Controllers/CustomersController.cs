using AssessmentApi.Errors;
using AssessmentApi.Models.Common;
using AssessmentApi.Models.Customers;
using AssessmentApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentApi.Controllers;

/// <summary>
/// Provides customer lookup endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class CustomersController(CustomerStore customers) : ControllerBase
{
    /// <summary>
    /// Gets a customer by identifier.
    /// </summary>
    /// <param name="id">Customer identifier.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The requested customer.</returns>
    [HttpGet("{id:int}", Name = "GetCustomerById")]
    [ProducesResponseType(typeof(ApiResponse<CustomerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CustomerResponse>>> GetById(int id, CancellationToken cancellationToken)
    {
        var customer = await customers.GetByIdAsync(id, cancellationToken);
        if (customer is null)
        {
            return NotFound(ApiErrorResponse.Create(HttpContext, StatusCodes.Status404NotFound, "Customer was not found."));
        }

        return Ok(new ApiResponse<CustomerResponse> { Data = customer, TraceId = HttpContext.TraceIdentifier });
    }
}
