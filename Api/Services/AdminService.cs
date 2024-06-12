using Api.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Api.Services
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _httpClient;
        private readonly MyDbContext dbContext;
        public AdminService(HttpClient httpClient, MyDbContext dbContext)
        {
            _httpClient = httpClient;
            this.dbContext = dbContext;
        }

        
        /*public async Task<ResponseModel> AdminLogin(LoginModel loginModel)
{
   var result = await _httpClient.PostAsJsonAsync("api/admin/AdminLogin", loginModel);
   var content = await result.Content.ReadAsStringAsync();
   var loginRespond = JsonSerializer.Deserialize<ResponseModel>(content);


}*/
    }
}
