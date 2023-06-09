using Microsoft.AspNetCore.Authentication.Cookies;
using SignalRWebApp.Hubs;
using SignalRWebApp.Service;
using SqlSugar;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//添加SignalR服务
builder.Services.AddSignalR();
// 添加Session服务
builder.Services.AddSession();

// 添加身份验证服务
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();


// 注册SqlSugar服务
builder.Services.AddTransient<ISqlSugarClient>(provider =>
{
    SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
    {
        ConnectionString = builder.Configuration.GetSection("conn").Value,//连接符字串
        DbType = DbType.Sqlite, //数据库类型
        IsAutoCloseConnection = true //不设成true要手动close
    });
    return db;
});
// 注册用户服务
builder.Services.AddTransient<IUserService, UserService>();

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
// 在 app.UseRouting() 和 app.UseEndpoints() 之间添加 UseAuthentication 和 UseAuthorization 中间件
app.UseAuthentication();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//配置
app.MapHub<ChatHub>("/chatHub");
app.MapHub<ChatOneFriend>("/chatOneFriend");
// 使用Session
app.UseSession();
app.Run();

