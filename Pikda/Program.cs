﻿using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows.Forms;

namespace Pikda
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            Application.Run(ServiceProvider.GetRequiredService<MainForm>());
        }
        public static IServiceProvider ServiceProvider { get; private set; }
        static IHostBuilder CreateHostBuilder()
        {
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services.AddDbContext<AppDbContext>(op=>op.UseSqlite("Data Source=Pikda.db"));
                    services.AddTransient<OcrService>();
                    services.AddTransient<MainForm>();
                });
        }
    }
}
