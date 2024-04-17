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
        public async Task CreateUser(CreateUserDTO createUserDTO)
        {
            try
            {
                //속성 유효성검사
                //if(createUserDTO == null)
                //업무규칙 적용
                //DTO와 Entity로 변경.

                // Configure AutoMapper

                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<CreateUserDTO, USER>());

                // Perform mapping
                Mapper mapper = new Mapper(configuration);
                USER user = mapper.Map<CreateUserDTO, USER>(createUserDTO);
                await loginMapper.Create(user);
                //Response DTO 생성후 Controller로 전달
            }
            catch (Exception ex)
            {

            }

        }

        public async Task<List<GetAllUserResponseDTO>> GetAllUser()
        {
            try
            {
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<USER, GetAllUserResponseDTO>());

                List<USER> list = await loginMapper.GetAll();


                Mapper mapper = new Mapper(configuration);
                List<GetAllUserResponseDTO> dtoList = mapper.Map<List<USER>, List<GetAllUserResponseDTO>>(list);
                return dtoList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<GetUserResponseDTO> GetUser(GetUserDTO getUserDTO)
        {
            //var configuration = new MapperConfiguration(cfg => { });



            //Mapper mapper = new Mapper(configuration);
            //Dictionary<string, object>  dc = mapper.Map<GetUserDTO, Dictionary<string, object>>(getUserDTO);
            //dc.Remove("Password");
            //ProcCall procCall = new ProcCall();
            //DataTable dt = await procCall.RequestProcedure("sp_usertest",dc);

            GetUserResponseDTO dto = new GetUserResponseDTO();
            //dto.Id = (int)dt.Rows[0]["id"];
            //dto.Userid = dt.Rows[0]["userid"].ToString();
            //dto.Username = dt.Rows[0]["username"].ToString();
            //dto.Point = (int)dt.Rows[0]["point"];
            return dto;
        }

    }
}
