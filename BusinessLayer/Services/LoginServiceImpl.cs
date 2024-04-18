using AutoMapper;
using BusinessLayer.DTO;
using DataAccessLayer.Mappers;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class LoginServiceImpl : ILoginService
    {
        ILoginMapper loginMapper;
        public LoginServiceImpl(ILoginMapper mapper)
        {
            loginMapper = mapper;
        }

        public async Task<bool> Login(GetUserDTO getUserDTO)
        {
            var configuration = new MapperConfiguration(cfg => { });
            Mapper mapper = new Mapper(configuration);
            Dictionary<string, object> dc = mapper.Map<GetUserDTO, Dictionary<string, object>>(getUserDTO);
            ProcCall procCall = new ProcCall();
            DataTable dt = await procCall.RequestProcedure("UpsertUserByEmail", dc);

            bool result = dt.Rows[0]["Result"].ToString() == "fail" ? false : true;
            return result;
        }

    }
}
