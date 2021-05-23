using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using panel_logowania_wersja_od_początku;

namespace panel_logowania_v4 {
	class DeterminingUser {
		public static List<Users> list = new List<Users>();
		public static string filePath = @"DL8978.bin", loggedIn;
		public static int loggedInRange;
		public static bool firstAdmin;

		public static void UpdateDatabase() {                                   //serialisation
			FileStream fs = new FileStream(filePath, FileMode.Create);
			BinaryFormatter bf = new BinaryFormatter();
			try {
				bf.Serialize(fs, list);
			}
			catch (SerializationException e) {
				Console.WriteLine("Failed to serialize. Reason: " + e.Message);
			}
			finally {
				fs.Close();
			}
		}

		public static void LoadFromFile() {                                     //deserialisation
			FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
			try {
				BinaryFormatter formatter = new BinaryFormatter();
				list = (List<Users>)formatter.Deserialize(fs);
			}
			catch (SerializationException e) {
				MessageBox.Show(e.Message);
			}
			finally {
				fs.Close();
			}
		}

		public static void AddUser(string Username, string Password, int range) {
			Boolean isCreatedUser = false;  //variable checking existing the user with his username
			firstAdmin = false;
			int lastSeenId = 0;
			foreach (Users user in DeterminingUser.list) {    //listing users in the listbox
				if (user.Username == Username)
					isCreatedUser = true;

				if (user.UserId > lastSeenId)
					lastSeenId = user.UserId;
			}


			if (isCreatedUser == false) {
				byte[] salt = Encoding.ASCII.GetBytes("8978Luba");
				Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(Password, salt);
				Password = Encoding.ASCII.GetString(pbkdf2.GetBytes(salt.Length));
				list.Add(new Users(Username, Password, ++lastSeenId, range));	 //another user has id increased
				MessageBox.Show("User '" + Username + "' created");              //unless the file would be deleted,
				UpdateDatabase();                                                //then incrementing is going to start from 1
			} else
				MessageBox.Show("User '" + Username + "' has already existed");
		}

		public static void ChangingPassword(string Username, string Password) {
			byte[] salt = Encoding.ASCII.GetBytes("8978Luba");
			Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(Password, salt);
			Password = Encoding.ASCII.GetString(pbkdf2.GetBytes(salt.Length));
			for (int userIndex = 0; userIndex < list.Count; userIndex++)           //loop looking for the entered user
				if (list[userIndex].Username == Username) {
					list[userIndex].Password = Password;
					UpdateDatabase();
					break;
				}                                     
		}

		public static Users Authorisation(string username, string password) {
			for (int userIndex = 0; userIndex < list.Count; userIndex++) {          //loop looking for the entered user
				if (list[userIndex].Username == username) {
					if (list[userIndex].IsValidPassword(list[userIndex].Password, password)) {
						Portal portal = new Portal();
						portal.LabelWelcomingText.Content += "\n"+ list[userIndex].Username;
						loggedInRange = list[userIndex].Range;
						loggedIn = username;
						//Enum.GetName(typeof(Users.Ranges), (int)list[userIndex].Range )	- name of range

						if (list[userIndex].Range == 99) {//opening welcoming window for
							portal.LabelPrivileges.Content += " an admin";			
						} 
						else if (list[userIndex].Range == 1) {
							portal.LabelPrivileges.Content += " a moderator";		   
						}
						else if (list[userIndex].Range == 0) {
							portal.LabelPrivileges.Content += " an user";         
							portal.PictureBoxEditingUsers.Visibility = Visibility.Hidden;                       
							portal.LabelEditingUsers.Visibility = Visibility.Hidden;
						}
						portal.Show();
						break;
					}
				} else if (userIndex == list.Count - 1)
					MessageBox.Show("Think twice before you press 'log in'");
			}
			return null;
		}

	}
}
