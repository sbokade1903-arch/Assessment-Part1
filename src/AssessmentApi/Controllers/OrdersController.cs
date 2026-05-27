using AssessmentApi.Errors;
using AssessmentApi.Models.Common;
using AssessmentApi.Models.Orders;
using AssessmentApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentApi.Controllers;

/// <summary>
    /// Handles order APIs.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class OrdersController(OrderStore orders) : ControllerBase
{
    /// <summary>
    /// Gets an order by id.
    /// </summary>
    /// <param name="id">Order id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("{id:int}", Name = "GetOrderById")]
    [ProducesResponseType(typeof(ApiResponse<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<OrderResponse>>> GetById(int id, CancellationToken cancellationToken)
    {
        var order = await orders.GetByIdAsync(id, cancellationToken);
        if (order is null)
        {
            return NotFound(ApiErrorResponse.Create(HttpContext, StatusCodes.Status404NotFound, "Order was not found."));
        }

        return Ok(new ApiResponse<OrderResponse> { Data = order, TraceId = HttpContext.TraceIdentifier });
    }

    /// <summary>
    /// Creates an order.
    /// </summary>
    /// <param name="request">Order details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost(Name = "CreateOrder")]
    [ProducesResponseType(typeof(ApiResponse<OrderResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<OrderResponse>>> Create(
        CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var order = await orders.CreateAsync(request, cancellationToken);
        if (order is null)
        {
            return BadRequest(ApiErrorResponse.Create(
                HttpContext,
                StatusCodes.Status400BadRequest,
                "The order references an unknown customer or product."));
        }

        var response = new ApiResponse<OrderResponse>
        {
            Data = order,
            Message = "Order created.",
            TraceId = HttpContext.TraceIdentifier
        };

        return CreatedAtRoute("GetOrderById", new { id = order.Id }, response);
    }
}
