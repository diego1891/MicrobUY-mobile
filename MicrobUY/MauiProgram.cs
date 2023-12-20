using CommunityToolkit.Maui;
using MicrobUY.Services;
using MicrobUY.ViewModels;
using MicrobUY.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Plugin.Firebase;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Shared;
using Plugin.Firebase.Android;

namespace MicrobUY
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var assemblyInstance = Assembly.GetExecutingAssembly();
            using var stream = assemblyInstance.GetManifestResourceStream("MicrobUY.appsettings.json");

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .RegisterFirebaseServices()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


           



            builder.Configuration.AddConfiguration(config);

            builder.Services.AddSingleton<INavegacionService, NavegacionService>();
            builder.Services.AddSingleton<SecurityService>();
            builder.Services.AddSingleton<PostsService>();
            builder.Services.AddSingleton<UsuarioService>();
            builder.Services.AddSingleton<BusquedaService>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();

            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<HomePage>();

            builder.Services.AddTransient<BusquedaViewModel>();
            builder.Services.AddTransient<BusquedaPage>();

            builder.Services.AddTransient<PostDetailViewModel>();
            builder.Services.AddTransient<PostDetailPage>();

            builder.Services.AddTransient<UsuarioDetailViewModel>();
            builder.Services.AddTransient<UsuarioDetailPage>();

            builder.Services.AddTransient<EscribirPostViewModel>();
            builder.Services.AddTransient<EscribirPostPage>();


            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(BusquedaPage), typeof(BusquedaPage));
            Routing.RegisterRoute(nameof(PostDetailPage), typeof(PostDetailPage));
            Routing.RegisterRoute(nameof(UsuarioDetailPage), typeof(UsuarioDetailPage));
            Routing.RegisterRoute(nameof(EscribirPostPage), typeof(EscribirPostPage));


            builder.Services.AddSingleton(Connectivity.Current);
            builder.Services.AddSingleton<HttpClient>();

            /*builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyCxBzvPHr85GTaM8a9SDKS53rzx72B2l2c",
                AuthDomain= "microbuy-5860f.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider(),
                    new GoogleProvider().AddScopes("email"),
                    new FacebookProvider().AddScopes("email"),
                    new GithubProvider().AddScopes("email")
                }
            }));*/


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
        private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
        {
            builder.ConfigureLifecycleEvents(events =>
            {

            events.AddAndroid(android => android.OnCreate((activity, state) =>
                    CrossFirebase.Initialize(activity, state, CreateCrossFirebaseSettings())));
            });

            builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);


            return builder;
        }

        private static CrossFirebaseSettings CreateCrossFirebaseSettings()
        {
            return new CrossFirebaseSettings(isAuthEnabled: true);
        }

    }
}