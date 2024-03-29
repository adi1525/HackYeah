﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

namespace BoshGUI.Blocks {
	/// <summary>
	/// Logika interakcji dla klasy Pause.xaml
	/// </summary>
	public partial class PauseBlock : UserControl, Iblock {

		public PauseBlock () {
			InitializeComponent ();
		}

		private void ValBox_PreviewTextInput (object sender, TextCompositionEventArgs e) {
			e.Handled = IsTextAllowed (e.Text);
			if (e.Text.Contains ("\r")) {
				MainWindow.SetFocus ();
			}
		}

		private void ValBox_TextChanged (object sender, TextChangedEventArgs e) {
			TextBox sb = sender as TextBox;
			if (sb.Text.Length == 0) {
				return;
			}
			if (int.Parse (sb.Text.Trim ('\r', ' ')) > 10_000) {
				( sender as TextBox ).Text = "10000";
			} else {
				sb.Text = sb.Text.Trim ('\r', ' ');
			}

		}

		private static readonly Regex _regex = new Regex ("[^0-9]+"); //regex that matches disallowed text
		private static bool IsTextAllowed (string text) {
			return _regex.IsMatch (text);
		}
		public byte GetBlockId () => 4;

		public int GetBlockData () => valBox.Text == null ? 0 : int.Parse (valBox.Text);
		public UserControl parent;
		public Iblock GetParent () => parent as Iblock;
		public void SetParent (Iblock p) => parent = p as UserControl; UserControl control;

		public Iblock GetChild () => control as Iblock;

		public Iblock GetLowestChild () => control == null ? this : ( control as Iblock ).GetLowestChild ();

		public void AddBlock (Iblock u) => control = u as UserControl;

		public Thickness GetMargin () => Margin;

		public void SetMargin (Thickness t) => Margin = t;

		private void UserControl_MouseDoubleClick (object sender, MouseButtonEventArgs e) {
			MainWindow.RemChi (this);
			if (control == null) {
				( parent as Iblock ).AddBlock (null);
			} else {
				( parent as Iblock ).AddBlock (control as Iblock);
				( control as Iblock ).SetParent (parent as Iblock);
				control = null;
			}
		}
	}
}
