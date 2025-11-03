using BuildingBlocks.Pagination;
using Discount.Grpc.DTOs;
using Discount.Grpc.Features.Coupons.Commands.CreateCoupon;
using Discount.Grpc.Features.Coupons.Commands.DeleteCoupon;
using Discount.Grpc.Features.Coupons.Commands.UpdateCoupon;
using Discount.Grpc.Features.Coupons.Queries.GetCouponById;
using Discount.Grpc.Features.Coupons.Queries.GetCouponsPaged;
using Discount.Grpc.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Discount.Grpc.Controllers;

/// <summary>
/// REST API Controller for managing discount coupons.
/// Provides CRUD operations with filtering, pagination, and status management.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CouponsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new discount coupon.
    /// </summary>
    /// <param name="createDto">The coupon creation data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created coupon.</returns>
    /// <response code="201">Coupon created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CouponDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CouponDto>> CreateCoupon(
        [FromBody] CreateCouponDto createDto,
        CancellationToken cancellationToken)
    {
        var command = new CreateCouponCommand(createDto);
        var result = await mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(
            nameof(GetCouponById),
            new { id = result.Id },
            result);
    }

    /// <summary>
    /// Gets a coupon by its ID.
    /// </summary>
    /// <param name="id">The coupon ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The coupon details.</returns>
    /// <response code="200">Coupon found.</response>
    /// <response code="404">Coupon not found.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CouponDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CouponDto>> GetCouponById(
        int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetCouponByIdQuery(id);
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Coupon with ID {id} not found" });
        }
    }

    /// <summary>
    /// Gets a paginated and filtered list of coupons.
    /// </summary>
    /// <param name="status">Optional status filter (Active, Expired, Disabled, Upcoming).</param>
    /// <param name="productName">Optional product name filter.</param>
    /// <param name="couponCode">Optional coupon code filter.</param>
    /// <param name="isActive">Optional active status filter.</param>
    /// <param name="pageNumber">Page number (default: 1).</param>
    /// <param name="pageSize">Page size (default: 10).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of coupons.</returns>
    /// <response code="200">List of coupons retrieved successfully.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<CouponDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<CouponDto>>> GetCoupons(
        [FromQuery] CouponStatus? status = null,
        [FromQuery] string? productName = null,
        [FromQuery] string? couponCode = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var filter = new CouponFilterDto
        {
            Status = status,
            ProductName = productName,
            CouponCode = couponCode,
            IsActive = isActive,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetCouponsPagedQuery(filter);
        var result = await mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing coupon.
    /// </summary>
    /// <param name="id">The coupon ID to update.</param>
    /// <param name="updateDto">The update data (only provided fields will be updated).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated coupon.</returns>
    /// <response code="200">Coupon updated successfully.</response>
    /// <response code="404">Coupon not found.</response>
    /// <response code="400">Invalid request data.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CouponDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CouponDto>> UpdateCoupon(
        int id,
        [FromBody] UpdateCouponDto updateDto,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateCouponCommand(id, updateDto);
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Coupon with ID {id} not found" });
        }
    }

    /// <summary>
    /// Disables a coupon (soft delete).
    /// Sets IsActive to false without deleting the coupon from the database.
    /// </summary>
    /// <param name="id">The coupon ID to disable.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The disabled coupon.</returns>
    /// <response code="200">Coupon disabled successfully.</response>
    /// <response code="404">Coupon not found.</response>
    [HttpPatch("{id:int}/disable")]
    [ProducesResponseType(typeof(CouponDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CouponDto>> DisableCoupon(
        int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateDto = new UpdateCouponDto { IsActive = false };
            var command = new UpdateCouponCommand(id, updateDto);
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Coupon with ID {id} not found" });
        }
    }

    /// <summary>
    /// Enables a previously disabled coupon.
    /// Sets IsActive to true.
    /// </summary>
    /// <param name="id">The coupon ID to enable.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The enabled coupon.</returns>
    /// <response code="200">Coupon enabled successfully.</response>
    /// <response code="404">Coupon not found.</response>
    [HttpPatch("{id:int}/enable")]
    [ProducesResponseType(typeof(CouponDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CouponDto>> EnableCoupon(
        int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateDto = new UpdateCouponDto { IsActive = true };
            var command = new UpdateCouponCommand(id, updateDto);
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Coupon with ID {id} not found" });
        }
    }

    /// <summary>
    /// Permanently deletes a coupon from the database.
    /// Use with caution - this action cannot be undone.
    /// Consider using the disable endpoint for soft deletion instead.
    /// </summary>
    /// <param name="id">The coupon ID to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Success status.</returns>
    /// <response code="204">Coupon deleted successfully.</response>
    /// <response code="404">Coupon not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCoupon(
        int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteCouponCommand(id);
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Coupon with ID {id} not found" });
        }
    }
}
