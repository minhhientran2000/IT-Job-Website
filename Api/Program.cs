using Api.Models;
using Api.Models.Chat;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
IronPdf.License.LicenseKey = "IRONSUITE.TRANMINHHIEN13122000.GMAIL.COM.30464-1AD997E131-FFW2K-GS6FAZTLOATZ-Z5PWLM75JJ7H-GURYO5THGM2F-GQUSRB6BLM4A-ST3PBE4U6STE-XEFA26QGAO5A-5NR5R6-TWXNK22EO22MUA-DEPLOYMENT.TRIAL-IB3M54.TRIAL.EXPIRES.11.JUL.2024";
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
