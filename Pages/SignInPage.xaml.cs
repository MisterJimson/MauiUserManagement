using Client = Supabase.Client;
using System.Runtime.CompilerServices;

namespace MauiUserManagement
{
	public partial class SignInPage : ContentPage
	{
		private string email;
		public string Email
		{
			get => email;
			set => SetField(ref email, value);
		}

		private string password;
		public string Password
		{
			get => password;
			set => SetField(ref password, value);
		}

		private string passwordConfirm;
		public string PasswordConfirm
		{
			get => passwordConfirm;
			set => SetField(ref passwordConfirm, value);
		}

		private string passwordError;
		public string PasswordError
		{
			get => passwordError;
			set => SetField(ref passwordError, value);
		}

		public SignInPage()
		{
			InitializeComponent();
			BindingContext = this;
		}

		async void OnSignInClicked(object sender, EventArgs e)
        {
			try
			{
				var test = Client.Instance.Auth; 
				var session1 = await Client.Instance.Auth.SignUp(Email, Password);
				//var session2 = await Client.Instance.Auth.SignIn(Email, Password);
				Console.WriteLine(session1.AccessToken);
			} catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
            }
		}

		protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return false;
			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}
	}
}