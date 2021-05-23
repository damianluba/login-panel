using panel_logowania_v4;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;

namespace panel_logowania_wersja_od_początku {
	/// <summary>
	/// Interaction logic for LoggingWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window {

		public LoginWindow() {
			InitializeComponent();                                  //here are startup commands
			if (File.Exists(DeterminingUser.filePath))
				DeterminingUser.LoadFromFile();
			else
				DeterminingUser.AddUser("Damian", "Łuba", 99);    //adding the first admin to first lauch program
			this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			PasswordBox.Visibility = Visibility.Hidden;
		}

		public bool IsTheTextBoxesFilledIn() {
			if ((TextBoxLogin.Text == "") || (PasswordBox.Password == ""))
				return false;
			else
				return true;
		}
		private void ButtonLogIn_Click(object sender, RoutedEventArgs e) {
			if (IsTheTextBoxesFilledIn())
				DeterminingUser.Authorisation(TextBoxLogin.Text, PasswordBox.Password);
			else
				MessageBox.Show("Fill in the fields", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
		}
		private void TextBoxLogin_MouseDown(object sender, MouseButtonEventArgs e) {
			TextBoxLogin_GotFocus(sender, e);
		}

		private void TextBoxLogin_GotFocus(object sender, RoutedEventArgs e) {
			TextBoxLogin_SecondLookSettings();

		}
		private void TextBoxPassword_GotFocus(object sender, RoutedEventArgs e) {
			PasswordBox.Visibility = Visibility.Visible;
			PasswordBox.Focus();
		}
		private void TextBoxLogin_LostFocus(object sender, RoutedEventArgs e) {
			TextBoxLogin_FirstLookSettings();
			Password_SecondLookSettings();
		}

		private void PasswordBox_LostFocus(object sender, RoutedEventArgs e) {
			if (PasswordBox.Password == "")
				PasswordBox.Visibility = Visibility.Hidden;
			else
				TextBoxPassword.Visibility = Visibility.Visible;
		}

		private void CompanyImage_MouseDown(object sender, MouseButtonEventArgs e) {
			System.Diagnostics.Process.Start("www.yahoo.com");
		}
		//------------------functions steering text visibilty on the texboxes--------------------------------//
		public void TextBoxLogin_FirstLookSettings() { //the same TextBoxLogin look as at the beginning
			if (TextBoxLogin.Text == "") {                                   //in case the TextBoxLogin has no entered string,
				TextBoxLogin.Foreground = SystemColors.HighlightBrush;             //visible text of the TextBoxLogin 
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
				PasswordBox.Visibility = Visibility.Hidden;                 //similar working as in TextBoxLogin_SecondLookSettings function 
			}
		}//-------------------------------------------------------------------------------------------------------------

	}
}
