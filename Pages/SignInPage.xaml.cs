using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Supabase;
using Supabase.Gotrue;
using Client = Supabase.Client;
using Newtonsoft.Json;
using System.Diagnostics;


namespace MauiUserManagement
{
	public partial class SignInPage : ContentPage
	{
		public SignInPage()
		{
			InitializeComponent();
		}

		async void OnSignInClicked(object sender, EventArgs e)
        {
			await Client.Instance.Auth.SendMagicLink(EmailEntry.Text);
		}
	}
}