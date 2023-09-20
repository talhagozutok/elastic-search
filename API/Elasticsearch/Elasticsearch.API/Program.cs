using Elasticsearch.API.Extensions;
using Elasticsearch.API.Repositories;
using Elasticsearch.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddElasticClient(builder.Configuration);
builder.Services.AddScoped(typeof(ProductService));
builder.Services.AddScoped(typeof(ProductRepository));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();
app.MapAllEndpoints();

app.Run();
