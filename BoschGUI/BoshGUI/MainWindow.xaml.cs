using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using BoshGUI.Blocks;
namespace BoshGUI {
	public delegate void SetFoc ();
	/// <summary>
	/// Logika interakcji dla klasy MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		public static SetFoc SetFocus;
		public static RemoveChild RemChi;


		public Iblock start;

		public MainWindow () {
			InitializeComponent ();
			SetFocus = ssFoc;
			start = new StartBlock ();
			RemChi = RemoChil;
			( start as UserControl ).Margin = new Thickness (0, 0, 0, 0);
			master.Children.Add (start as UserControl);
			ssFoc ();
			Commands.Start ();
		}

		private void ssFoc () {
			master.Focus ();
		}

		private void Button_Click (object sender, RoutedEventArgs e) {
			AddBlock (new TurnBlock ());
		}

		private void Button_Click_1 (object sender, RoutedEventArgs e) {
			AddBlock (new StopBlock ());
		}

		private void Button_Click_2 (object sender, RoutedEventArgs e) {
			AddBlock (new SpeedBlock ());
		}

		private void Button_Click_3 (object sender, RoutedEventArgs e) {
			AddBlock (new PauseBlock ());
		}

		private void Button_Click_4 (object sender, RoutedEventArgs e) {
			AddBlock (new BaseBlock ());
		}
		private void AddBlock (Iblock u) {
		//	if (u.GetType () == typeof (TurnBlock)) {
				//		FocusManager.SetFocusedElement (master, ( u as TurnBlock ).valBox);
			//			( u as TurnBlock ).valBox.Focus ();
			//			Keyboard.Focus (( u as TurnBlock ).valBox);
			//			( u as TurnBlock ).valBox.SelectAll ();
		//	}
			Iblock w = start.GetLowestChild ();
			w.AddBlock (u);
			u.SetParent (w);
			Thickness t = w.GetMargin ();
			u.SetMargin (new Thickness (t.Left, t.Top + 60, 0, 0));
			master.Children.Add (u as UserControl);
		}

		List<int> values;
		List<Wololo> commands;

		Thread task;
		private void Button_Click_Compile (object sender, RoutedEventArgs e) {
			values = new List<int> ();
			commands = new List<Wololo> ();

			Iblock b = start;
			int aa = 0;
			while (!( b == null )) {
				values.Add (b.GetBlockData ());
				aa = b.GetBlockId ();
				switch (aa) {
					case 0:
						commands.Add (Commands.Init);
						break;
					case 1:
						commands.Add (Commands.Turn);
						break;
					case 2:
						commands.Add (Commands.Stop);
						break;
					case 3:
						commands.Add (Commands.Speed);
						break;
					case 4:
						commands.Add (Commands.Pause);
						break;
					case 5:
						commands.Add (Commands.Base);
						break;
				}

				b = b.GetChild ();
			}
			if (task != null) {
				task.Abort ();
			}
			task = new Thread (Execute);
			task.Start ();
		}

		private void Execute () {
			for (int i = 0; i != commands.Count; i++) {
				commands[i].Invoke (values[i]);
			}
		}
		private void Button_Click_Focus (object sender, RoutedEventArgs e) {
			ssFoc ();
		}

		private void Button_KeyDown (object sender, KeyEventArgs e) {
			FocusManager.GetFocusedElement (this);
		}

		private void Button_Click_DeleteAll (object sender, RoutedEventArgs e) {
			master.Children.Clear ();
			start = new StartBlock ();
			( start as UserControl ).Margin = new Thickness (0, 0, 0, 0);
			master.Children.Add (start as UserControl);
		}
		private void RemoChil (UserControl a) {
			master.Children.Remove (a);
			Iblock ai = ( a as Iblock ).GetChild ();

			while (ai != null) {
				Thickness t = ai.GetMargin ();
				ai.SetMargin (new Thickness (t.Left, t.Top - 60, 0, 0));
				ai = ai.GetChild ();
			}
		}
	}
	public delegate void Wololo (int Wululu);
	public delegate void RemoveChild (UserControl uc);
}
