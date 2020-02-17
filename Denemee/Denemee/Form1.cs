using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstagramApiSharp;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Enums;
using InstagramApiSharp.Helpers;
using InstagramApiSharp.Logger;
using System.IO;

namespace Denemee
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static IInstaApi api;
        private static UserSessionData user;
        private string zaman = "";
        private Bitmap bmp;
        private Bitmap bmp1;
        private Bitmap bmp2;
        private Bitmap bmp3;

        private void Button1_Click(object sender, EventArgs e)
        {
            Dogrula(api);
        }

        private async void BtnBaslat_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtKullaniciAdi.Text))
                {
                    MessageBox.Show("Kullanıcı Adı Boş Olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(txtParola.Text))
                {
                    MessageBox.Show("Parola Boş Olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UserSessionData userSessionData = new UserSessionData();
                user = userSessionData;
                user.UserName = txtKullaniciAdi.Text;
                user.Password = txtParola.Text;
                api = InstaApiBuilder.CreateBuilder().SetUser(user).UseLogger(new DebugLogger(LogLevel.Exceptions)).Build();
                

                var logInResult = await api.LoginAsync();
                if (logInResult.Succeeded)
                {
                    timer1.Start();
                }
                else
                {
                    if (logInResult.Value == InstaLoginResult.ChallengeRequired)
                    {
                        var challenge = await api.GetChallengeRequireVerifyMethodAsync();
                        if (challenge.Succeeded)
                        {
                            if (challenge.Value.SubmitPhoneRequired)
                            {
                                //Telefon ile işlem
                            }
                            else
                            {
                                if (challenge.Value.StepData != null)
                                {
                                    if (!string.IsNullOrEmpty(challenge.Value.StepData.PhoneNumber))
                                    {
                                        var pnv = challenge.Value.StepData.PhoneNumber;
                                        MessageBox.Show(pnv);
                                        SendCode(api);
                                    }
                                    if (!string.IsNullOrEmpty(challenge.Value.StepData.Email))
                                    {
                                        var emv = challenge.Value.StepData.Email;
                                        MessageBox.Show(emv);
                                        SendCode(api);
                                    }

                                }
                            }
                        }
                        else
                            MessageBox.Show(challenge.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void SendCode(IInstaApi api)
        {
            bool isEmail = true;
            try
            {
                if (isEmail)
                {
                    var email = await api.RequestVerifyCodeToEmailForChallengeRequireAsync();
                    this.Size = new Size(429, 260);
                    if (email.Succeeded)
                    {
                        MessageBox.Show("Gönderildi");
                    }
                    else
                        MessageBox.Show(email.Info.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var phoneNumber = await api.RequestVerifyCodeToSMSForChallengeRequireAsync();
                    if (phoneNumber.Succeeded)
                    {
                        MessageBox.Show("Doğrulandı");
                        this.Size = new Size(429, 200);
                    }
                    else
                        MessageBox.Show(phoneNumber.Info.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public async void Dogrula(IInstaApi api)
        {
            try
            {
                var verifyLogin = await api.VerifyCodeForChallengeRequireAsync(txtDogrulama.Text);
                if (verifyLogin.Succeeded)
                {
                    MessageBox.Show("Doğrulandı");
                    this.Size = new Size(429, 200);
                    timer1.Start();
                }
                else
                {
                    MessageBox.Show("Yanlış kod");
                    if (verifyLogin.Value == InstaLoginResult.TwoFactorRequired)
                    {
                        MessageBox.Show("iki faktörlü doğrulama gerekiyor.");
                    }
                    else
                        MessageBox.Show(verifyLogin.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "D");
            }
        }
        int sayi = 0;

        private async void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                sayi++;
                if (sayi % 5 == 1)
                {
                    string dosyayolu = "resim.png";
                    bmp = new Bitmap(dosyayolu);
                    pbPP.Image = bmp;
                    bmp.Save("resimpp.png");
                    string ppyolu = "resimpp.png";
                    byte[] bytes = File.ReadAllBytes(ppyolu);
                    await api.AccountProcessor.ChangeProfilePictureAsync(bytes);
                }
                if (sayi % 5 == 0)
                {
                    string dosyaYolu = "resim1.png";
                    bmp1 = new Bitmap(dosyaYolu);
                    pbPP.Image = bmp1;
                    bmp1.Save("resimPp.png");
                    string ppYolu = "resimPp.png";
                    byte[] Bytes = File.ReadAllBytes(ppYolu);
                    await api.AccountProcessor.ChangeProfilePictureAsync(Bytes);

                }
                if (sayi % 3 == 0)
                {
                    string dosyayolu = "resim2.png";
                    bmp2 = new Bitmap(dosyayolu);
                    pbPP.Image = bmp2;
                    bmp2.Save("resimPP.png");
                    string ppyolu = "resimPP.png";
                    byte[] bytes = File.ReadAllBytes(ppyolu);
                    await api.AccountProcessor.ChangeProfilePictureAsync(bytes);
                }
                if (sayi % 3 == 1)
                {
                    string dosyayolu = "resim3.png";
                    bmp3 = new Bitmap(dosyayolu);
                    pbPP.Image = bmp3;
                    bmp3.Save("ResimPP.png");
                    string ppyolu = "ResimPP.png";
                    byte[] bytes = File.ReadAllBytes(ppyolu);
                    await api.AccountProcessor.ChangeProfilePictureAsync(bytes);
                }

            }
            catch (Exception ex)
            {
                this.Text = "Hata: " + ex.Message;
            }
        }
    }
}
