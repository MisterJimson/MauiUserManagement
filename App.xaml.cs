using Supabase;
using Supabase.Gotrue;
using Client = Supabase.Client;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MauiUserManagement;

public partial class App : Application
{
    private string supabaseCacheFilename = ".supabase.cache";

    public App()
	{
		InitializeComponent();
        InitSupabase();

        MainPage = new SignInPage();
	}

    async void InitSupabase()
    {
        var options = new SupabaseOptions
        {
            SessionPersistor = SessionPersistor,
            SessionRetriever = SessionRetriever,
            SessionDestroyer = SessionDestroyer
        };

        Client.Initialize("https://mmkiepvuoiuldzrtknee.supabase.co", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiYW5vbiIsImlhdCI6MTY0MTc0MzcxOCwiZXhwIjoxOTU3MzE5NzE4fQ.Z-PnjrFdhODjqwmEInuIw6MnpT6DkgXWPBeH910TQ0E", options, (client) => {
            Debug.Print("Supabase Initialize Done");
        });
    }

    internal Task<bool> SessionPersistor(Session session)
    {
        try
        {
            var cacheDir = FileSystem.CacheDirectory;
            var path = Path.Join(cacheDir, supabaseCacheFilename);
            var str = JsonConvert.SerializeObject(session);

            using (StreamWriter file = new StreamWriter(path))
            {
                file.Write(str);
                file.Dispose();
                return Task.FromResult(true);

            };
        }
        catch (Exception)
        {
            Debug.WriteLine("Unable to write cache file.");
            throw;
        }
    }

    internal Task<Session> SessionRetriever()
    {
        var tsc = new TaskCompletionSource<Session>();
        try
        {
            var cacheDir = FileSystem.CacheDirectory;
            var path = Path.Join(cacheDir, supabaseCacheFilename);

            if (File.Exists(path))
            {
                using (StreamReader file = new StreamReader(path))
                {
                    var str = file.ReadToEnd();
                    if (!String.IsNullOrEmpty(str))
                        tsc.SetResult(JsonConvert.DeserializeObject<Session>(str));
                    else
                        tsc.SetResult(null);
                    file.Dispose();
                };
            }
            else
            {
                tsc.SetResult(null);
            }
        }
        catch
        {
            Debug.WriteLine("Unable to read cache file.");
            tsc.SetResult(null);
        }
        return tsc.Task;

    }

    internal Task<bool> SessionDestroyer()
    {
        try
        {
            var cacheDir = FileSystem.CacheDirectory;
            var path = Path.Join(cacheDir, supabaseCacheFilename);
            if (File.Exists(path))
                File.Delete(path);
            return Task.FromResult(true);
        }
        catch (Exception)
        {
            Debug.WriteLine("Unable to delete cache file.");
            return Task.FromResult(false);
        }
    }
}

