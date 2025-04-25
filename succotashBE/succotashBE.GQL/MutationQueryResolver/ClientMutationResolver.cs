using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JozyKQL.PG.Services.DynamicService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using succotashBE.data.Functions;
using succotashBE.data.Model;

namespace succotashBE.GQL.MutationQueryResolver
{
    [ExtendObjectType("Mutation")]
    public class ClientMutationResolver
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClientMutationResolver(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        private Userinfo GetUserInfo()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue("Id");
            var roleIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue("RoleId");
            var roleGroupIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue("CountryId");
            var countryIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue("CountyId");
            var subcountuString = _httpContextAccessor.HttpContext?.User?.FindFirstValue("SubCountryId");


            return new Userinfo
            {
                UserId = int.TryParse(userIdString, out int userId) ? userId : (int?)null,
                RoleId = int.TryParse(roleIdString, out int roleId) ? roleId : (int?)null,
                CountryId = int.TryParse(roleGroupIdString, out int roleGroupId) ? roleGroupId : (int?)null,
                CountyId = int.TryParse(countryIdString, out int countyId) ? countyId : (int?)null,
                SubcountyId = int.TryParse(subcountuString, out int subcountyId) ? subcountyId : (int?)null,
            };
        }

        private DynamicService CreatedDynamicService
        {
            get
            {
                return new DynamicService(_configuration.GetConnectionString("ConnectionString")!);
            }
        }

        #region Accounts

        //waking up services
        public async Task<FEOutput> CreateClient(CLIENTS client)
        {
            try
            {
                var userInfo = GetUserInfo();
                
                var result = await CreatedDynamicService.CreateDynamically(client);
                return new FEOutput { Results = JsonConvert.SerializeObject(result) };
            }
            catch (Exception ex)
            {
                return new FEOutput { Error = JsonConvert.SerializeObject(ex) };
            }
        }


        public async Task<FEOutput> UpdateClient(CLIENTS client)
        {
            try
            {
                var userInfo = GetUserInfo();
                var result = await CreatedDynamicService.UpdateDynamically(client);
                return new FEOutput { Results = JsonConvert.SerializeObject(result) };
            }
            catch (Exception ex)
            {
                return new FEOutput { Error = JsonConvert.SerializeObject(ex) };
            }
        }
        #endregion

    }
}
