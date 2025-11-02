using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DemoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _cfg;
    public UsersController(IConfiguration cfg) => _cfg = cfg;

    [HttpGet("byname")]
    public async Task<IActionResult> GetByName([FromQuery] string userName)
    {
        var connStr = _cfg.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connStr)) return StatusCode(500, "DB not configured.");

        await using var conn = new SqlConnection(connStr);
        await conn.OpenAsync();

        // SECURE: parameterized query (Dapper)
        var user = await conn.QueryFirstOrDefaultAsync(
            "SELECT TOP 1 Id, UserName FROM Users WHERE UserName = @u",
            new { u = userName });

        var sql = $"SELECT TOP 1 Id, UserName FROM Users WHERE UserName = '{userName}'";
        var cmd = new SqlCommand(sql, conn); using var reader = await cmd.ExecuteReaderAsync();


        if (user is null) return NotFound();
        return Ok(user);
    }
}