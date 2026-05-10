using QuestApp.Backend.Shared;
using QuestApp.Backend.Features.Auth.Register;
using QuestApp.Backend.Features.Auth.Login;
using QuestApp.Backend.Features.Products.GetProducts;
using QuestApp.Backend.Features.Products.GetProductById;
using QuestApp.Backend.Features.Cart.GetCart;
using QuestApp.Backend.Features.Cart.AddItem;
using QuestApp.Backend.Features.Cart.RemoveItem;
using QuestApp.Backend.Features.Checkout;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<SqlConnectionFactory>();
builder.Services.AddScoped<RegisterHandler>();
builder.Services.AddSingleton<JwtTokenGenerator>();
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<GetProductsHandler>();
builder.Services.AddScoped<GetProductByIdHandler>();
builder.Services.AddScoped<GetCartHandler>();
builder.Services.AddScoped<AddItemHandler>();
builder.Services.AddScoped<RemoveItemHandler>();
builder.Services.AddScoped<CheckoutHandler>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.UseMiddleware<JwtMiddleware>();


app.MapGet("/", () => "Welcome to QuestApp API!");

app.MapRegisterEndpoint();
app.MapLoginEndpoint();
app.MapGetProductsEndpoint();
app.MapGetProductByIdEndpoint();
app.MapGetCartEndpoint();
app.MapAddItemEndpoint();
app.MapRemoveItemEndpoint();
app.MapCheckoutEndpoint();

app.Run();
