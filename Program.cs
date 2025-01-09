using PokemonApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // Add services for controllers
builder.Services.AddEndpointsApiExplorer(); // Enable endpoint exploration
builder.Services.AddSwaggerGen(); // Add Swagger generation

builder.Services.AddScoped<IPokemonService, PokemonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.MapControllers(); // Map attribute routes from controllers
app.Run();