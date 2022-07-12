namespace ChurchManagementPortal
{
    public class User
    {
        public enum Priviledges
        {
            Admin,
            Secretary,
            User
        }
        public int UserId { get; private set; }
        public string Username { get; private set; }
        private string Password { get; set; }
        public string Priviledge { get; private set; }

        public User(int userId, string username, string password, string priviledge)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Priviledge = priviledge;
        }

        public void changeUsername(string username)
        {
            Username = username;
        }

        public void changePassword(string password)
        {
            Password = password;
        }

        public void changePriviledge(string priviledge)
        {
            Priviledge = priviledge;
        }
    }
}
