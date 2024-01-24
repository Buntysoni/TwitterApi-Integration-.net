using Tweetinvi;
using Tweetinvi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ITwitterClient>(sp =>
{
    // You can customize the configuration here based on your needs
    var consumerKey = "xxxxxxxxxxxxxxxxxxxx";
    var consumerSecret = "xxxxxxxxxxxxxxxxxxxx";
    var accessToken = "xxxxxxxxxxxxxxxxxxxx";
    var accessTokenSecret = "xxxxxxxxxxxxxxxxxxxx";

    var credentials = new TwitterCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
    var twitterClient = new TwitterClient(credentials);
    return twitterClient;
});
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
