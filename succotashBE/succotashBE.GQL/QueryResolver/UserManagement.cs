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
using succotashBE.data.Views;

namespace succotashBE.GQL.QueryResolver
{
    [ExtendObjectType("Query")]
    public class UserManagement
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserManagement(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
                UserId = int.TryParse(userIdString, out int userId) ? userId : null,
                RoleId = int.TryParse(roleIdString, out int roleId) ? roleId : null,
                CountryId = int.TryParse(roleGroupIdString, out int roleGroupId) ? roleGroupId : null,
                CountyId = int.TryParse(countryIdString, out int countyId) ? countyId : null,
                SubcountyId = int.TryParse(subcountuString, out int subcountyId) ? subcountyId : null,
            };
        }

        private DynamicService CreatedDynamicService
        {
            get
            {
                return new DynamicService(_configuration.GetConnectionString("ConnectionString")!);
            }
        }

        #region Users

        //Getting all Users from the country
        public async Task<FEOutput> GetUsers()
        {
            try
            {
                // Getting Information stored in the token
                var userInfo = GetUserInfo();
                UsersView usersView = new();
                string whereclause = $"CountryId = {userInfo.CountryId}";
                var result = await CreatedDynamicService.GetBySpecificWhereClauseDynamically(usersView, whereclause);
                return new FEOutput { Results = JsonConvert.SerializeObject(result) };
            }
            catch (Exception ex)
            {
                return new FEOutput { Error = JsonConvert.SerializeObject(ex) };
            }
        }

        // Fetching user By Id
        public async Task<FEOutput?> GetUserById(UsersView userView)
        {
            try
            {
                var userInfo = GetUserInfo();
                string WhereClause = $"Id = {userView.Id}";
                var result = await CreatedDynamicService.GetBySpecificWhereClauseDynamically(userView, WhereClause);
                return new FEOutput { Results = JsonConvert.SerializeObject(result) };
            }
            catch (Exception ex)
            {

                return new FEOutput { Error = JsonConvert.SerializeObject(ex) }; ;
            }
        }
       
        #endregion
    }
}