using System.Diagnostics;
using System;

public static class TimerManager
{
	private static Stopwatch stopwatch;

	public static void ResetTimer ()
	{
		stopwatch = new Stopwatch();
	}

	public static void PauseTimer ()
	{
		stopwatch.Stop();
	}

	public static void ResumeTimer ()
	{
		stopwatch.Start();
	}

	public static void StartTimer ()
	{
		stopwatch.Start();
	}

	public static TimeSpan GetTimeSpan ()
	{
		TimeSpan ts = stopwatch.Elapsed;
		return ts;
	}

	public static string GetTimespanString ()
	{
		TimeSpan ts = stopwatch.Elapsed;
		return ts.ToString();
	}

	public static string GetTimerString ()
	{
		if (stopwatch == null)
			return "";
		TimeSpan ts = stopwatch.Elapsed;
		return String.Format("{0:00}:{1:00}.{2:00}",
			ts.Minutes, ts.Seconds,
			ts.Milliseconds / 10);
	}
}
