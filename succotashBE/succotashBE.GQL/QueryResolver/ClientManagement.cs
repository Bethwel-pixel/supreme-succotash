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
    public class ClientManagement
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClientManagement(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
                SubcountyId= int.TryParse(subcountuString, out int subcountyId) ? subcountyId : (int?)null,
            };
        }

        private DynamicService CreatedDynamicService
        {
            get
            {
                return new DynamicService(_configuration.GetConnectionString("ConnectionString")!);
            }
        }

        #region Clients

        //Getting all clients from the country
        public async Task<FEOutput> GetClients()
        {
            try
            {
                var userInfo = GetUserInfo();
                Clientsview clients = new();
                string whereclause = $"CountryId = {userInfo.CountryId}";
                var result = await CreatedDynamicService.GetBySpecificWhereClauseDynamically(clients, whereclause);
                return new FEOutput { Results = JsonConvert.SerializeObject(result) };
            }
            catch (Exception ex)
            {
                return new FEOutput { Error = JsonConvert.SerializeObject(ex) };
            }
        }

        // Fetching client By Id
        public async Task<FEOutput?> GetClientById(Clientsview clients)
        {
            try
            {
                var userInfo = GetUserInfo();
                String WhereClause = $"Id = {clients.Id}";
                var result = await CreatedDynamicService.GetBySpecificWhereClauseDynamically(clients, WhereClause);
                return new FEOutput { Results = JsonConvert.SerializeObject(result) };
            }
            catch (Exception ex)
            {
                
                return new FEOutput { Error = JsonConvert.SerializeObject(ex) }; ;
            }
        } 
        
        // Getting all client programs
        public async Task<FEOutput> GetClientPrograms()
        {
            try
            {
                var userInfo = GetUserInfo();
                ClientPROGRAMS clientProgram = new();
                string whereclause = $"CountryId = {userInfo.CountryId}";
                var result = await CreatedDynamicService.GetBySpecificWhereClauseDynamically(clientProgram, whereclause);
                return new FEOutput { Results = JsonConvert.SerializeObject(result) };
            }
            catch (Exception ex)
            {
                return new FEOutput { Error = JsonConvert.SerializeObject(ex) };
            }
        }


        public async Task<FEOutput?> GetClientProgramById(ClientProgramsView clientsProgram)
        {
            try
            {
                var userInfo = GetUserInfo();
                String WhereClause = $"Id = {clientsProgram.Clientid}";
                var result = await CreatedDynamicService.GetBySpecificWhereClauseDynamically(clientsProgram, WhereClause);
                return new FEOutput { Results = JsonConvert.SerializeObject(result) };
            }
            catch (Exception ex)
            {
                
                return new FEOutput { Error = JsonConvert.SerializeObject(ex) }; ;
            }
        }
        #endregion

        #region Setups
        // fetching all countries
        public async Task<FEOutput?> getallCountry()
        {
            try
            {
                COUNTRY cOUNTRY = new COUNTRY();
                var results = await CreatedDynamicService.GetAllDynamically(cOUNTRY);
                return new FEOutput { Results = JsonConvert.SerializeObject(results) };

            }
            catch (Exception ex)
            {
                return new FEOutput
                {
                    Error = JsonConvert.SerializeObject(ex)

                };
            }
        }
        public async Task<FEOutput?> getallCounties()
        {
            try
            {
                CountyView cOUNTy = new CountyView();
                var results = await CreatedDynamicService.GetAllDynamically(cOUNTy);
                return new FEOutput { Results = JsonConvert.SerializeObject(results) };

            }
            catch (Exception ex)
            {
                return new FEOutput
                {
                    Error = JsonConvert.SerializeObject(ex)

                };
            }
        }
        // Fetching Counties By Country
        public async Task<FEOutput?> getallCountiesByCountry(CountyView cOUNTY)
        {
            try
            {
                var userInfo = GetUserInfo();
                string whereclause = $"Countryid={userInfo.CountryId}";
                var results = await CreatedDynamicService.GetBySpecificWhereClauseDynamically(cOUNTY, whereclause);
                return new FEOutput { Results = JsonConvert.SerializeObject(results) };

            }
            catch (Exception ex)
            {
                return new FEOutput
                {
                    Error = JsonConvert.SerializeObject(ex)

                };
            }
        }
        
        public async Task<FEOutput?> getallSubCountie()
        {
            try
            {
                SubCountyView cOUNTy = new SubCountyView();
                var results = await CreatedDynamicService.GetAllDynamically(cOUNTy);
                return new FEOutput { Results = JsonConvert.SerializeObject(results) };

            }
            catch (Exception ex)
            {
                return new FEOutput
                {
                    Error = JsonConvert.SerializeObject(ex)

                };
            }
        }

        // Fetching Subcounty by CountyId
        public async Task<FEOutput?> getSubCountyByCountyid(SubCountyView subcOUNTY)
        {
            try
            {
                var userInfo = GetUserInfo();
                string whereclause = $"Countryid={subcOUNTY.Countyid}";
                var results = await CreatedDynamicService.GetBySpecificWhereClauseDynamically(subcOUNTY, whereclause);
                return new FEOutput { Results = JsonConvert.SerializeObject(results) };

            }
            catch (Exception ex)
            {
                return new FEOutput
                {
                    Error = JsonConvert.SerializeObject(ex)

                };
            }
        } 

        //fetching all Gender
        public async Task<FEOutput?> getallGender(GendeR gender)
        {
            try
            {
                var userInfo = GetUserInfo();
                var results = await CreatedDynamicService.GetAllDynamically(gender);
                return new FEOutput { Results = JsonConvert.SerializeObject(results) };

            }
            catch (Exception ex)
            {
                return new FEOutput
                {
                    Error = JsonConvert.SerializeObject(ex)

                };
            }
        } 
        
        // Fetching Roles
        public async Task<FEOutput?> getallRoles(Roles roles)
        {
            try
            {
                var userInfo = GetUserInfo();
                var results = await CreatedDynamicService.GetAllDynamically(roles);
                return new FEOutput { Results = JsonConvert.SerializeObject(results) };

            }
            catch (Exception ex)
            {
                return new FEOutput
                {
                    Error = JsonConvert.SerializeObject(ex)

                };
            }
        }

        #endregion


    }
}
