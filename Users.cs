using System;
using System.Text;
using System.Security.Cryptography;

namespace panel_logowania_v4 {
    [Serializable]
    class Users {
        public string Username, Password;
        public int UserId, Range;

        public enum Ranges {
            user=0, moderator, admin=99
        }

        public Users(string Username, string Password, int UserId, int Range) {
            this.Username = Username;
            this.Password = Password;
            this.UserId = UserId;
            this.Range=Range;
        }

        public bool IsValidPassword(string Password, string enteredPassword) {          //checks password validity 
            byte[] salt = Encoding.ASCII.GetBytes("8978Luba");
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt);
            enteredPassword = Encoding.ASCII.GetString(pbkdf2.GetBytes(salt.Length));
            return (Password == enteredPassword) ? true : false;
        }

        public int getRange() {
            return Range;
        }
    }
}
