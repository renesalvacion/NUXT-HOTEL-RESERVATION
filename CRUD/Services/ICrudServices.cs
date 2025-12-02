using CRUD.DTO;
using CRUD.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CRUD.Services
{
    public interface ICrudServices
    {
        Task<(bool Success, string Message)> PostCrud(CrudDto dto, string fileName);
        Task<(bool Success, string Message, string Token)> LoginServ(LoginDto dto);
        Task<(bool Success, Crud? User)> ViewProfile(int userId);
        Task<IEnumerable<Crud>> GetData();

        //sign in using google auth
        Task<(string message, List<GoogleUserInfo> api)> GoogleSignIn([FromBody] GoogleAuthRequest googleAuth);
        Task<(string message, bool Success, List<GoogleUserInfo> api, string token)> GoogleLogin([FromBody] GoogleAuthRequest googleAuth);
    }
}
