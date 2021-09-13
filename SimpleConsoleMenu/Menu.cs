using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleConsoleMenu
{
	public interface IMenuItem
	{
		void Select(IMenu invokedFrom);
		void PrintItem();
	}

	public interface IMenu : IMenuItem
	{
		IMenu GetPreviousMenu();
	}
	public class MenuItem : IMenuItem
	{
		protected string itemText;
		Action action;
		public MenuItem(string itemText, Action action)
		{
			this.itemText = itemText;
			this.action = action;
		}

		public void PrintItem()
		{
			Console.WriteLine(itemText);
		}

		public void Select(IMenu invokedFrom)
		{
			action?.Invoke();
			Console.ReadKey();
			invokedFrom.Select(invokedFrom.GetPreviousMenu());
		}
	}
	public class Menu : IMenu
	{
		protected string menuHeader;
		IMenuItem[] menuItems;
		public IMenu PreviousMenu { get; private set; }
		public IMenu GetPreviousMenu() => PreviousMenu;

		readonly string selectAgainText = "Incorrect input. Try again.",
			selectText = "Please select option using number in range [FROM, TO]:",
			toPrevMenuText = "Return to previous menu",
			exitText = "Exit";

		public Menu(string menuHeader, IMenuItem[] menuItems)
		{
			this.menuHeader = menuHeader;
			this.menuItems = menuItems;
		}

		public Menu(string menuHeader, IMenuItem[] menuItems,
			string selectAgainText, string selectText, string toPrevMenuText, string exitText) : this(menuHeader, menuItems)
		{
			this.selectAgainText = selectAgainText;
			this.selectText = selectText;
			this.toPrevMenuText = toPrevMenuText;
			this.exitText = exitText;
		}

		public void PrintItem()
		{
			Console.WriteLine(menuHeader);
		}

		void PrintMenu()
		{
			Console.Clear();
			Console.WriteLine(menuHeader);
			Console.WriteLine();
			int counter = 1;
			foreach (var it in menuItems)
			{
				Console.Write(counter++ + "). ");
				it.PrintItem();
			}
			Console.WriteLine();
			Console.WriteLine("0). " + (PreviousMenu == null ? exitText : toPrevMenuText));
			Console.WriteLine();
			Console.WriteLine(selectText.Replace("FROM", "1")
				.Replace("TO", menuItems.Length.ToString()));
		}

		int GetUserSelectedItemIndex()
		{
			int selectedIndex = -1;
			do
			{
				if (!int.TryParse(Console.ReadLine(), out selectedIndex)
					|| selectedIndex < 0 || selectedIndex > menuItems.Length)
				{
					selectedIndex = -1;
					Console.WriteLine(selectAgainText);
					Console.ReadKey();
					PrintMenu();
				}
			} while (selectedIndex < 0);
			return selectedIndex;
		}

		public void Select(IMenu invokedFrom = null)
		{
			if (invokedFrom != null)
				PreviousMenu = invokedFrom;
			PrintMenu();
			int selection = GetUserSelectedItemIndex();
			if (selection != 0)
				menuItems[selection - 1]?.Select(this);
			else
				PreviousMenu?.Select(null);
		}
	}
}
