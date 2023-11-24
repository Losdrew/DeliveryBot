using ClosedXML.Excel;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Shared.Dto.Error;
using DeliveryBot.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBot.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataController
{
    private readonly ApplicationDbContext _context;

    public DataController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("export-data")]
    [Authorize(Roles = Roles.Administrator)]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(typeof(ErrorDto), 400)]
    [FileDownload(FileName = "Data.xlsx")]
    public async Task<IActionResult> ExportData()
    {
        var addresses = _context.Addresses.ToList();
        var companies = _context.Companies.ToList();
        var companyEmployees = _context.CompanyEmployees.ToList();
        var customers = _context.Customers.ToList();
        var deliveries = _context.Deliveries.ToList();
        var orders = _context.Orders.ToList();
        var orderProducts = _context.OrderProducts.ToList();
        var products = _context.Products.ToList();
        var robots = _context.Robots.ToList();

        using (var workbook = new XLWorkbook())
        {
            AddWorksheet(workbook, "Addresses", addresses);
            AddWorksheet(workbook, "Companies", companies);
            AddWorksheet(workbook, "CompanyEmployees", companyEmployees);
            AddWorksheet(workbook, "Customers", customers);
            AddWorksheet(workbook, "Deliveries", deliveries);
            AddWorksheet(workbook, "Orders", orders);
            AddWorksheet(workbook, "OrderProducts", orderProducts);
            AddWorksheet(workbook, "Products", products);
            AddWorksheet(workbook, "Robots", robots);

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return new FileContentResult(content, "application/vnd.ms-excel");
            }
        }
    }

    private void AddWorksheet<T>(IXLWorkbook workbook, string worksheetName, IList<T> data)
    {
        var worksheet = workbook.Worksheets.Add(worksheetName);
        worksheet.Cell(1, 1).InsertTable(data);
    }
}