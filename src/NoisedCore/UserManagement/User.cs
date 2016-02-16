namespace Noised.Core.UserManagement
{
    /// <summary>
    /// A user using noised
    /// </summary>
    public class User
    {
        #region Properties

        public string Name { get; set; }

        public string Password { get; set; }

        #endregion

        #region Constructors

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }
    };
}
