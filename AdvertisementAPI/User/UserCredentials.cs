namespace AdvertisementAPI.User
{
    public class UserCredentials
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel()
            {
                UserName = "fabian_admin",
                Email = "fabian_admin@email.se",
                Password = "123",
                Givenname = "Fabian",
                Surname = "Bech Persson",
                Role = "Admin"
            },
            new UserModel()
            {
                UserName = "fabian_user",
                Email = "fabian_user@email.se",
                Password = "123",
                Givenname = "Fabian",
                Surname = "Bech Persson",
                Role = "User"
            }
        };
    }
}
