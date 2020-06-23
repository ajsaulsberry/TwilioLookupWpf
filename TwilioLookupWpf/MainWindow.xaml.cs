using System;
using System.Diagnostics;
using System.Windows;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Lookups.V1;
using TwilioLookupWpf.Models;

namespace TwilioLookupWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var twilioSettings = new TwilioSettings
            {
                AccountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID"),
                AuthToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN")
            };

            TwilioClient.Init(twilioSettings.AccountSid, twilioSettings.AuthToken);
            InitializeComponent();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtPhoneRaw.Text))
            {
                try
                {
                    lblStatus.Content = "Verifying with Twilio";
                    var phoneNumber = await PhoneNumberResource.FetchAsync(
                        countryCode: txtCountryCode.Text,
                        pathPhoneNumber: new Twilio.Types.PhoneNumber(txtPhoneRaw.Text));
                    txtPhoneVerified.Text = phoneNumber.PhoneNumber.ToString();
                    if (!String.IsNullOrEmpty(txtPhoneVerified.Text))
                    {
                        lblStatus.Content = "Verified by Twilio";
                    }
                }
                catch (ApiException apiex)
                {
                    Debug.WriteLine($"Twilio API Error {apiex.Code} - {apiex.MoreInfo}");
                    lblStatus.Content = $"{apiex.Message}";
                }
            }
        }
    }
}
