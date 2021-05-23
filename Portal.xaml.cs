using panel_logowania_v4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace panel_logowania_wersja_od_początku {
	/// <summary>
	/// Interaction logic for Portal.xaml
	/// </summary>
	public partial class Portal: Window {
		public Portal() {
			InitializeComponent();
		}

		private void ButtonLogOut_Click(object sender, RoutedEventArgs e) {
			Environment.Exit(0);
		}

		private void PictureBoxEditingUsers_MouseDown(object sender, MouseButtonEventArgs e) {
			DeterminingUser.UpdateDatabase();
			UsersEditingWindow usersEditingWindow = new UsersEditingWindow();
			if (DeterminingUser.loggedInRange == 1) {
				usersEditingWindow.ButtonAddTheUser.Visibility = Visibility.Hidden;
				usersEditingWindow.ButtonRemoveTheUser.Visibility = Visibility.Hidden;
				usersEditingWindow.RadioButtonAdmin.Visibility = Visibility.Hidden;
				usersEditingWindow.RadioButtonModerator.Visibility = Visibility.Hidden;
				usersEditingWindow.RadioButtonUser.Visibility = Visibility.Hidden;
				usersEditingWindow.GroupBoxRanges.Visibility = Visibility.Hidden;
			}
			usersEditingWindow.Show();
		}
	}
}
