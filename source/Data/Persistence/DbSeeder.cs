using Data.EF;
using Data.Enums;
using Data.Models;
using Data.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
        {
            await dbContext.Database.MigrateAsync(cancellationToken);

            if (!await.dbContext.AppIdeas.AnyAsync(cancellationToken))
            {
                dbContext.AppIdeas.AddRange(
                    new AppIdea(AppType.Web, AppCategory.Health, "Personalized workout planner"),
                    new AppIdea(AppType.Web, AppCategory.Finance, "Expense tracker"),
                    new AppIdea(AppType.Web, AppCategory.Productivity, "AI-powered task scheduler")
                );                
            }

            if (!await dbContext.Palettes.AnyAsync(cancellationToken))
            {
                dbContext.Palettes.AddRange(
                    new Palette(HexColor.Parse("#FF5733"), HexColor.Parse("#FFC300"), HexColor.Parse("#DAF7A6")),
                    new Palette(HexColor.Parse("#0077BE"), HexColor.Parse("#00A9E0"), HexColor.Parse("#00CED1")),
                    new Palette(HexColor.Parse("#2E8B57"), HexColor.Parse("#3CB371"), HexColor.Parse("#66CDAA"))
                );                
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}