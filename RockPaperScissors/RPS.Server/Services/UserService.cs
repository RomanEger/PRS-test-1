using System.Net;
using System.Text.Json;
using Entities.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Shared.Helpers;

namespace RockPaperScissors.Services;
 
public class UserService(IConfiguration config) : RockPaperScissors.UserService.UserServiceBase
{
    public override Task<ListReply> ListUsers(Empty request, ServerCallContext context) =>
        Task.Run(async () =>
        {
            using var httpClient = new HttpClient();

            var httpResponseMessage = await httpClient.GetAsync($"{config.GetConnectionString("ApiConnection")}/api/users");

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                throw new Exception(httpResponseMessage.Content.ToString());
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            var userList = JsonSerializer.Deserialize<IEnumerable<User>>(content, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            
            if (userList == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Список пользователей пуст"));
            
            var userReplyList = new List<UserReply>();
            
            foreach (var user in userList)
            {
                userReplyList.Add(new UserReply()
                {
                    Id = user.Id,
                    Login = user.Login,
                    Password = user.Password,
                    Balance = Convert.ToDouble(user.Balance)
                });
            }
            
            var listReply = new ListReply();
            
            listReply.Users.AddRange(userReplyList);
            
            return listReply;
        });
    

    public override Task<UserReply> GetUser(GetUserRequest request, ServerCallContext context) =>
        Task.Run(async () =>
        {
            using var httpClient = new HttpClient();
            
            HttpResponseMessage httpResponseMessage = new();
            
            if(request.Id > 0) 
                httpResponseMessage = await httpClient.GetAsync($"{config.GetConnectionString("ApiConnection")}/api/users/id{request.Id}");
            else if(!string.IsNullOrWhiteSpace(request.Login))
                httpResponseMessage = await httpClient.GetAsync($"{config.GetConnectionString("ApiConnection")}/api/users/login{request.Login}");
            
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                throw new Exception(httpResponseMessage.Content.ToString());
            
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            var userReply = JsonSerializer.Deserialize<UserReply>(content, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            
            if (userReply == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Пользователь не найден"));

            if (request.Password != PasswordHash.DecodeFrom64(userReply.Password))
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Некорректный пароль"));
            
            return userReply;
        });

    
    public override Task<UserReply> CreateUser(CreateUserRequest request, ServerCallContext context) =>
        Task.Run(async () =>
        {
            using var httpClient = new HttpClient();
            
            var user = new User()
            {
                Login = request.Login,
                Password = request.Password,
                Balance = 0
            };
            
            var httpResponseMessage = await httpClient.PostAsJsonAsync($"{config.GetConnectionString("ApiConnection")}/api/users", user);

            if (httpResponseMessage.StatusCode != HttpStatusCode.Created)
                throw new RpcException(new Status(StatusCode.Aborted, "Не удалось создать пользователя"));
            
            return new UserReply(); 
        });


    public override Task<UserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context) =>
        Task.Run(async () =>
        {
            using var httpClient = new HttpClient();
            
            var user = new User()
            {
                Id = request.Id,
                Login = request.Login,
                Password = request.Password,
                Balance = Convert.ToDecimal(request.Balance)
            };
            
            var httpResponseMessage = await httpClient.PutAsJsonAsync($"{config.GetConnectionString("ApiConnection")}/api/users", user);

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                throw new RpcException(new Status(StatusCode.Aborted, "Не удалось обновить пользователя"));
            
            return new UserReply(); 
        });
                  
    public override Task<UserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context) =>
        Task.Run(async () =>
        {
            using var httpClient = new HttpClient();
            
            var httpResponseMessage = await httpClient.DeleteAsync($"{config.GetConnectionString("ApiConnection")}/api/users/{request.Id}");

            if (httpResponseMessage.StatusCode != HttpStatusCode.NoContent)
                throw new RpcException(new Status(StatusCode.Aborted, "Не удалось удалить пользователя"));
            
            return new UserReply(); 
        });
    
}