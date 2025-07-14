using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AzureDbTestApp.Data;
using AzureDbTestApp.Models;

namespace AzureDbTestApp.Pages
{
    public class DatabaseTestModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseTestModel> _logger;

        public DatabaseTestModel(ApplicationDbContext context, ILogger<DatabaseTestModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<SampleItem> Items { get; set; } = new();
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = false;

        public async Task OnGetAsync()
        {
            await LoadItemsAsync();
        }

        public async Task<IActionResult> OnPostAddItemAsync(string name, string description)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    Message = "Name is required.";
                    IsSuccess = false;
                    await LoadItemsAsync();
                    return Page();
                }

                var item = new SampleItem
                {
                    Name = name.Trim(),
                    Description = description?.Trim() ?? string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.SampleItems.Add(item);
                await _context.SaveChangesAsync();

                Message = $"Item '{name}' added successfully!";
                IsSuccess = true;
                _logger.LogInformation("Item added: {Name}", name);
            }
            catch (Exception ex)
            {
                Message = $"Error adding item: {ex.Message}";
                IsSuccess = false;
                _logger.LogError(ex, "Error adding item");
            }

            await LoadItemsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteItemAsync(int id)
        {
            try
            {
                var item = await _context.SampleItems.FindAsync(id);
                if (item != null)
                {
                    _context.SampleItems.Remove(item);
                    await _context.SaveChangesAsync();
                    Message = $"Item '{item.Name}' deleted successfully!";
                    IsSuccess = true;
                    _logger.LogInformation("Item deleted: {Id}", id);
                }
                else
                {
                    Message = "Item not found.";
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                Message = $"Error deleting item: {ex.Message}";
                IsSuccess = false;
                _logger.LogError(ex, "Error deleting item");
            }

            await LoadItemsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostClearItemsAsync()
        {
            try
            {
                var items = await _context.SampleItems.ToListAsync();
                _context.SampleItems.RemoveRange(items);
                await _context.SaveChangesAsync();

                Message = $"All {items.Count} items cleared successfully!";
                IsSuccess = true;
                _logger.LogInformation("All items cleared");
            }
            catch (Exception ex)
            {
                Message = $"Error clearing items: {ex.Message}";
                IsSuccess = false;
                _logger.LogError(ex, "Error clearing items");
            }

            await LoadItemsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostTestConnectionAsync()
        {
            try
            {
                // Test database connection
                await _context.Database.OpenConnectionAsync();
                await _context.Database.CloseConnectionAsync();

                // Test table creation
                await _context.Database.EnsureCreatedAsync();

                // Test a simple query
                var count = await _context.SampleItems.CountAsync();

                Message = $"Database connection successful! Current item count: {count}";
                IsSuccess = true;
                _logger.LogInformation("Database connection test successful");
            }
            catch (Exception ex)
            {
                Message = $"Database connection failed: {ex.Message}";
                IsSuccess = false;
                _logger.LogError(ex, "Database connection test failed");
            }

            await LoadItemsAsync();
            return Page();
        }

        private async Task LoadItemsAsync()
        {
            try
            {
                Items = await _context.SampleItems
                    .OrderByDescending(i => i.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading items");
                Items = new List<SampleItem>();
            }
        }
    }
}
