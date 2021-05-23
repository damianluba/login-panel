using panel_logowania_v4;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace panel_logowania_wersja_od_początku {
	/// <summary>
	/// Interaction logic for UsersEditingWindow.xaml
	/// </summary>
	public partial class UsersEditingWindow: Window {
		public UsersEditingWindow() {
			InitializeComponent();
			foreach (Users user in DeterminingUser.list)       //listing users on the listbox
				ListBoxUsers.Items.Add(user.Username);
			PasswordBoxPassword.Visibility = Visibility.Hidden;
			PasswordBoxRepeatPassword.Visibility = Visibility.Hidden;
			PasswordBoxPassword.ToolTip = null;
			PasswordBoxRepeatPassword.ToolTip = null;
			ButtonCloseChangingPasswordPart.Visibility = Visibility.Hidden;
		}
		//string selectedRange;

		private void ButtonAddTheUser_Click(object sender, RoutedEventArgs e) {
			if (IsTheTextBoxesFilledIn()) {
				if (IsIdenticalPassword()) {
					ListBoxUsers.Items.Add(TextBoxLogin.Text);
					MessageBox.Show(getSelectedRange().ToString());
					DeterminingUser.AddUser(TextBoxLogin.Text, PasswordBoxPassword.Password, getSelectedRange());
				} else
					MessageBox.Show("Different passwords");
			} else
				MessageBox.Show("Fill in the fields", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		public int getSelectedRange() {
			if (RadioButtonAdmin.IsChecked == true) {
				//selectedRange = Enum.GetName(typeof(Users.Range), 99); 
				return (int)Users.Ranges.admin;                      //99
			} else if (RadioButtonModerator.IsChecked == true) {
				return (int)Users.Ranges.moderator;					//1
			} else 
				return (int)Users.Ranges.user;                       //0
		}

		private void ButtonRemoveTheUser_Click(object sender, RoutedEventArgs e) {
			string userToRemove = ListBoxUsers.SelectedItem.ToString();
			foreach (Users user in DeterminingUser.list) {                                                  //eventual find the user is finished with break
				if ((user.Username == "Damian") && (ListBoxUsers.SelectedItem.ToString() == "Damian")) {
					MessageBox.Show("You are not allowed to remove the first user-first admin", "Forbidden action", MessageBoxButton.OK, MessageBoxImage.Error);
				} 
				else if (DeterminingUser.loggedIn == userToRemove) {
					MessageBox.Show("Removing currently logged in user is irrational");
				}
				else if (user.Username == userToRemove) {
					MessageBoxResult decision = MessageBox.Show("Are you sure to remove " + userToRemove + "?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning,
															MessageBoxResult.No, MessageBoxOptions.RightAlign);

					if (decision == MessageBoxResult.Yes) {                 //admin confirms wish to remove the user
						DeterminingUser.list.Remove(user);                          //removing a user from database
						ListBoxUsers.Items.RemoveAt(ListBoxUsers.SelectedIndex);    //removing a user from listbox
						MessageBox.Show("'" + userToRemove + "'" + " has been removed from database");
						DeterminingUser.UpdateDatabase();
						break;
					}
				}
			}
		}

		public bool IsTheTextBoxesFilledIn() {      //once a user enters some string into textboxes the colors change
			if(TextBoxLogin.Foreground == SystemColors.HighlightBrush || PasswordBoxPassword.Foreground == SystemColors.HighlightBrush
								|| PasswordBoxRepeatPassword.Foreground == SystemColors.HighlightBrush)
				return false;
			else
				return true;
		}
		private bool IsIdenticalPassword() {
			return (PasswordBoxPassword.Password==PasswordBoxRepeatPassword.Password) ? true : false;
		}
		/*public bool IsAdmin() {             //assigninig admin privileges or their lack according to the checkbox
			if (CheckBoxAdminPrivileges.IsChecked == true)
				return true;
			else
				return false;
		}*/
		private void TextBoxLogin_GotFocus(object sender, RoutedEventArgs e) {
			TextBoxLogin_SecondLookSettings();
		}
		private void TextBoxPassword_GotFocus(object sender, RoutedEventArgs e) {
			PasswordBoxPassword.Visibility = Visibility.Visible;
			PasswordBoxPassword.Focus();
		}
		private void TextBoxRepeatPassword_GotFocus(object sender, RoutedEventArgs e) {
			PasswordBoxRepeatPassword.Visibility = Visibility.Visible;
			PasswordBoxRepeatPassword.Focus();
		}
		private void TextBoxLogin_LostFocus(object sender, RoutedEventArgs e) {
			TextBoxLogin_FirstLookSettings();
			Password_SecondLookSettings();
		}
		private void PasswordBoxPassword_LostFocus(object sender, RoutedEventArgs e) {
			if (PasswordBoxPassword.Password == "")
				PasswordBoxPassword.Visibility = Visibility.Hidden;
			else
				TextBoxPassword.Visibility = Visibility.Visible;
		}

		private void PasswordBoxRepeatPassword_LostFocus(object sender, RoutedEventArgs e) {
			if (PasswordBoxRepeatPassword.Password == "")
				PasswordBoxRepeatPassword.Visibility = Visibility.Hidden;
			else
				TextBoxRepeatPassword.Visibility = Visibility.Visible;
		}
		//------------------functions steering text visibilty on the texboxes--------------------------------//
		public void TextBoxLogin_FirstLookSettings() { //the same TextBoxLogin look as at the beginning
			if (TextBoxLogin.Text == "") {                                   //in case the TextBoxLogin has no entered string,
				TextBoxLogin.Foreground = SystemColors.HighlightBrush;       //visible text of the TextBoxLogin 
				TextBoxLogin.Text = "login";                                 //is just "login"
			}
		}
		private void TextBoxLogin_SecondLookSettings() {        //the look during entering text to TextBoxLogin
			if ((TextBoxLogin.Foreground == SystemColors.HighlightBrush) && (TextBoxLogin.Text == "login")) {
				TextBoxLogin.Clear();                                           //here is clearing the TextBoxLogin if cursor is set on its;
				TextBoxLogin.Foreground = Brushes.Black;
			}
		}
		private void Password_SecondLookSettings() {
			if (TextBoxPassword.Foreground == SystemColors.HighlightBrush) {
				PasswordBoxPassword.Visibility = Visibility.Hidden;                 //similar working as in TextBoxLogin_SecondLookSettings function 
			}
			if (TextBoxRepeatPassword.Foreground == SystemColors.HighlightBrush) {
				PasswordBoxRepeatPassword.Visibility = Visibility.Hidden;              
			}
		}//-------------------------------------------------------------------------------------------------------------

		private void CheckBoxAdminPrivileges_Checked(object sender, RoutedEventArgs e) {
			
		}

		private void HideAndShowOnRightPrivileges() {
			string userToEdit = ListBoxUsers.SelectedItem.ToString();
			if (DeterminingUser.loggedInRange == 99) {
				ButtonAddTheUser.Visibility = Visibility.Hidden;
				GroupBoxRanges.Visibility = Visibility.Hidden;
				RadioButtonAdmin.Visibility = Visibility.Hidden;
				RadioButtonModerator.Visibility = Visibility.Hidden;
				RadioButtonUser.Visibility = Visibility.Hidden;
				ButtonRemoveTheUser.Visibility = Visibility.Hidden;
			}
			ButtonCloseChangingPasswordPart.Visibility = Visibility.Visible;
			TextBoxLogin.Foreground = Brushes.Black;
			TextBoxLogin.Text = ListBoxUsers.SelectedItem.ToString();
			ListBoxUsers.Visibility = Visibility.Hidden;
			ButtonChangePassword.Visibility = Visibility.Hidden;
			ButtonChangePasswordFor.Content = "change " + userToEdit + "'s password";
			ButtonChangePasswordFor.Visibility = Visibility.Visible;
			TextBoxPassword.Text = "enter new password";
			TextBoxRepeatPassword.Text = "repeat new password";
		}

		private void ButtonChangePassword_Click(object sender, RoutedEventArgs e) {
			string userToEdit = ListBoxUsers.SelectedItem.ToString();
			if (DeterminingUser.loggedInRange != 99) {
				foreach (Users user in DeterminingUser.list) {
					if ((userToEdit == user.Username) && (user.Range == 99)) {       //no permission to change admins and moderators' passwords by moderators
						MessageBox.Show("You are not high enough to remove someone higher");
						break;
					} else { 
						HideAndShowOnRightPrivileges();
						ButtonCloseChangingPasswordPart.Visibility = Visibility.Hidden;
					}
				}
			} else	HideAndShowOnRightPrivileges();
		}

		private void ButtonChangePasswordFor_Click(object sender, RoutedEventArgs e) {
			string userToEdit = ListBoxUsers.SelectedItem.ToString();
			if (IsIdenticalPassword()) {
				CloseChangingPasswordPart();
				MessageBox.Show("Password updated");
			} else
				MessageBox.Show("Different passwords"); 
		}

		private void ButtonCloseChangingPasswortPart_Click(object sender, RoutedEventArgs e) {
			CloseChangingPasswordPart();
		}

		private void CloseChangingPasswordPart() {
			string userToEdit = ListBoxUsers.SelectedItem.ToString();
			if (DeterminingUser.loggedInRange == 99) {
				TextBoxPassword.Text = "password";
				TextBoxRepeatPassword.Text = "repeat password";
				ButtonAddTheUser.Visibility = Visibility.Visible;
				GroupBoxRanges.Visibility = Visibility.Visible;
				RadioButtonAdmin.Visibility = Visibility.Visible;
				RadioButtonModerator.Visibility = Visibility.Visible;
				RadioButtonUser.Visibility = Visibility.Visible;
			}
			ButtonCloseChangingPasswordPart.Visibility = Visibility.Hidden;
			ListBoxUsers.Visibility = Visibility.Visible;
			ButtonChangePassword.Visibility = Visibility.Visible;
			ButtonChangePasswordFor.Content = "change " + userToEdit + "'s password";
			ButtonChangePasswordFor.Visibility = Visibility.Hidden;
			DeterminingUser.ChangingPassword(TextBoxLogin.Text, PasswordBoxPassword.Password);
		}

		
	}
}