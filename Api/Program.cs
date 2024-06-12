using Api.Models;
using Api.Models.Chat;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
IronPdf.License.LicenseKey = "IRONSUITE.ANJAPAN12345.GMAIL.COM.31845-B60B5DADD9-O5G6E-NFYLMPLLIEU3-FHSMIDS7323I-U5HA5457O25D-R6XTJAOCXK6E-VHJ2VNORX3RF-5VEBTDSCI77E-PWOHFO-TUIN6TLLFD2MUA-DEPLOYMENT.TRIAL-PRZY27.TRIAL.EXPIRES.26.MAY.2024";
// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectString")));
//JSON Serializer
builder.Services.AddControllers().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson(
    options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

builder.Services.AddSignalR();
/*builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});*/
var app = builder.Build();
//Enable CORS
app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

/*app.MapHub<ChatHub>("/chatHub");*/

app.Run();
