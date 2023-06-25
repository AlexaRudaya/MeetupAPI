var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallServices(
    builder.Configuration,
    builder.Logging,
    typeof(IServiceInstaller).Assembly);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<MeetupSeed>>();

logger.LogInformation("Database migration running...");

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;

    try
    {
        var meetupContext = scopedProvider.GetRequiredService<MeetupContext>();

        if (meetupContext.Database.IsSqlServer())
        {
            meetupContext.Database.Migrate();
        }

        await MeetupSeed.SeedAsync(meetupContext, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred adding migrations to Database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();