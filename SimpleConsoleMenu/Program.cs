using System;

namespace SimpleConsoleMenu
{
	class Program
	{
		static IMenu menu = new Menu("Main menu", new IMenuItem[]
			{
				new Menu("Submenu", new IMenuItem[]
					{
						new MenuItem("Option FOO", ()=>Console.WriteLine("FOO!!!")),
						new Menu("Sub-submenu!", new IMenuItem[]
						{
							new MenuItem("Option BAR", ()=>Console.WriteLine("BAR!!!"))
						})
					}),
				new MenuItem("Option getRand", ()=>Console.WriteLine(new Random().Next(0, 100)))
			});
		static void Main(string[] args)
		{
			menu.Select(null);
		}
	}
}
