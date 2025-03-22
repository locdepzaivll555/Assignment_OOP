namespace addaccount.Interface
{
    public interface IUserManager
    {
        void SignUp(); // Định nghĩa khớp với SignUp()
        bool SignIn(string usernameOrEmail, string password);
        string RecoverPassword(string phone);

    }
}
