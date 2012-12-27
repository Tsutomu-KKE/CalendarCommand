using System;
using System.Collections.Generic;
using System.Text;

namespace Calendar
{
	class Program
	{
		static void Main(string[] args)
		{
			int mo = args.Length > 0 ? int.Parse(args[0]) : DateTime.Today.Month;
			int yr = args.Length > 1 ? int.Parse(args[1]) : DateTime.Today.Year;
			var hly = GetHoliday(yr);
			Console.WriteLine("{0,8} {1}", mo, yr);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("Su ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write("Mo Tu We Th Fr ");
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Sa ");
			var dt = new DateTime(yr, mo, 1);
			int ofs = (int)dt.DayOfWeek;
			for (int i = 0; i < ofs * 3; ++i) Console.Write(" ");
			int n = (dt.AddMonths(1) - dt).Days;
			for (int i = 0; i < n; ++i)
			{
				int k = (i + ofs) % 7;
				bool b = k == 0 || hly.ContainsKey(dt.AddDays(i));
				Console.ForegroundColor = b ? ConsoleColor.Red : (k == 6 ? ConsoleColor.Blue : ConsoleColor.White); 
				Console.Write("{0,2} ", i + 1);
				if (k == 6 || i == n - 1) Console.WriteLine();
			}
			Console.ResetColor();
		}
		/// <summary>
		/// <para>指定した年の日本の祝日の集合を返す</para>
		/// <para>ただし祝日が日曜にあたる場合、祝日は集合に含まず、代わりに振替休日を集合に含める</para>
		/// </summary>
		/// <param name="year">年</param>
		public static Dictionary<DateTime, object> GetHoliday(int year)
		{
			Dictionary<DateTime, object> dic = new Dictionary<DateTime, object>();
			int 春分の日 = (int)(20.8431 + 0.242194 * (year - 1980) - (int)((year - 1980) / 4));
			int 秋分の日 = (int)(23.2488 + 0.242194 * (year - 1980) - (int)((year - 1980) / 4));
			string holiday = "1/1,1/-2,2/11,3/" + 春分の日 + ",4/29,5/3,5/4,5/5,7/-3,9/-3,9/"
			  + 秋分の日 + ",10/-2,11/3,11/23,12/23";
			string[] ss = holiday.Split(',');
			Array.Reverse(ss);
			foreach (string s in ss)
			{
				string[] t = s.Split('/');
				int m = int.Parse(t[0]);
				int d = int.Parse(t[1]);
				DateTime dt = new DateTime(year, m, Math.Max(1, d));
				if (d < 0)
				{
					while (dt.DayOfWeek != DayOfWeek.Monday) dt = dt.AddDays(1);
					while (++d < 0) dt = dt.AddDays(7);
				}
				if (dt.DayOfWeek == DayOfWeek.Sunday) dt = dt.AddDays(1);
				while (dic.ContainsKey(dt)) dt = dt.AddDays(1);
				if (!dic.ContainsKey(dt.AddDays(1)) &&
					dic.ContainsKey(dt.AddDays(2)))
					dic.Add(dt.AddDays(1), null);
				dic.Add(dt, null);
			}
			return dic;
		}
	}
}
