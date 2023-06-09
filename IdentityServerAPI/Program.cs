var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityServer()
                        .AddInMemoryClients(IdentityConfiguration.Clients)
                        .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
                        .AddInMemoryApiResources(IdentityConfiguration.ApiResources)
                        .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
                        .AddTestUsers(IdentityConfiguration.TestUsers)
                        .AddDeveloperSigningCredential();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseIdentityServer();

app.MapControllers();

app.Run();