using Grpc.Net.Client;
using PRS.Client;

using var channel = GrpcChannel.ForAddress("https://localhost:7235");

while (true)
{
    Console.WriteLine("Выберите действие:\n" +
                      "1. Войти\n" +
                      "2. Зарегистрироваться");
    
    var ans = Console.ReadLine();
    if (ans != "1" && ans != "2")
    {
        Console.WriteLine("Некорректный ответ. Попробуйте еще раз");
        continue;
    }

    var client = new UserService.UserServiceClient(channel);

    while (true)
    {
        
        Console.Write("Введите логин: ");
        var login = Console.ReadLine();

        Console.Write("Введите пароль: ");
        var password = Console.ReadLine();

        if (ans == "2")
        {
            try
            {
                client.CreateUser(new CreateUserRequest()
                {
                    Login = login,
                    Password = password
                });
                Console.WriteLine("Регистрация прошла успешно");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\nПопробуйте еще раз");
                continue;
            }
        }

        try
        {
            var user = client.GetUser(new GetUserRequest()
            {
                Login = login,
                Password = password
            });
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message + "\nПопробуйте еще раз");
            continue;
        }
        
        Console.ReadKey();
        break;
    }
    break;
}

