﻿using Microsoft.Extensions.Logging;
using Plugin.Maui.KeyListener;

namespace WordleApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseKeyListener()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
			

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
