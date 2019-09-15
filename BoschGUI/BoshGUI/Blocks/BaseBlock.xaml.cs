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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BoshGUI.Blocks {
	/// <summary>
	/// Logika interakcji dla klasy BaseBlock.xaml
	/// </summary>
	public partial class BaseBlock : UserControl, Iblock {
		public BaseBlock () {
			InitializeComponent ();
		}

		public byte GetBlockId () => 5;

		public int GetBlockData () => 0;
		public UserControl control;
		public UserControl parent;
		public Iblock GetParent () => parent as Iblock;
		public void SetParent (Iblock p) => parent = p as UserControl;
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
