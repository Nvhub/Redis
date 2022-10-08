using Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDependencies();
builder.Services.AddServices(builder);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGraphQL();

app.MapGraphQLVoyager();

app.Run();
