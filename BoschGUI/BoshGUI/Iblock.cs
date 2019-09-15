using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace BoshGUI {
	public interface Iblock {
		byte GetBlockId ();
		int GetBlockData ();
		Iblock GetChild ();
		Iblock GetLowestChild ();

		void AddBlock (Iblock u);

		Thickness GetMargin ();

		void SetMargin (Thickness t);

		void SetParent (Iblock b);

		Iblock GetParent ();
	}
}
