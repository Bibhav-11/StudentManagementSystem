using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using SMSClient.Authentication.AuthorizationHandlers;
using SMSClient.Client;
using SMSClient.Data.Identity;
using SMSClient.Model;
using SMSClient.Repository;
using SMSClient.Repository.Students;
using SMSClient.Service.Courses;
using SMSClient.Service.Departments;
using SMSClient.Service.Classes;
using SMSClient.Service.Students;
using SMSClient.Service.Users;
using SMSClient.Service.Teahcers;
using SMSClient.Repository.Departments;
using SMSClient.Repository.Classes;
using SMSClient.Service.Attendances;
using Microsoft.Extensions.Configuration;
using SMSClient.Repository.Courses;
using SMSClient.Repository.Teachers;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

Log.Logger = new LoggerConfiguration().WriteTo.File("Logs/Serilog-.txt", rollingInterval: RollingInterval.Hour).CreateLogger();


builder.Services.AddDbContext<AspIdUsersContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDefaultIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<AspIdUsersContext>();

builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddEntityFrameworkStores<AspIdUsersContext>();

//Repository/Service
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<ICourseRepositiory, CourseRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();


//Attendance
builder.Services.AddScoped<IAttendanceService, AttendanceService>();    


//Authorization Handler
builder.Services.AddSingleton<IAuthorizationHandler, HasAccessPermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, HasAnyModuleAccessPermissionAuthorizationHandler>();

//Attendance Api
builder.Services.AddSingleton<IAttendanceClient, AttendanceClient>();
builder.Services.AddHttpClient<IAttendanceClient, AttendanceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7099/api/");
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookie";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookie", options =>
    {
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:5001";
        //options.GetClaimsFromUserInfoEndpoint = true;
        options.ClientId = "SMSClient";
        options.ClientSecret = "SMSSecret";
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("roles");
        options.Scope.Add("permissions");
        options.Scope.Add("attendance.full_access");
    });




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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization();

app.Run();
