using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DarkTradeManager
{
    public partial class AuthWindow : Window
    {
        private int failedAttempts = 0;
        private string currentCaptcha;

        public AuthWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtLogin.Text;
            string password = PwdPassword.Password;

            // Если капча отображается, проверяем её
            if (CaptchaArea.Visibility == Visibility.Visible)
            {
                if (TxtCaptcha.Text != currentCaptcha)
                {
                    MessageBox.Show("Капча неверна", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    GenerateCaptcha();
                    return;
                }
            }

            UserEntity user;
            using (var context = new DarkDbContext())
            {
                user = context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            }

            if (user != null)
            {
                DashboardWindow dash = new DashboardWindow(user);
                dash.Show();
                this.Close();
            }
            else
            {
                failedAttempts++;
                MessageBox.Show("Неверные учетные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (failedAttempts >= 1)
                {
                    CaptchaArea.Visibility = Visibility.Visible;
                    GenerateCaptcha();
                }
            }
        }

        private void GenerateCaptcha()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rnd = new Random();
            currentCaptcha = new string(Enumerable.Repeat(chars, 4)
                                    .Select(s => s[rnd.Next(s.Length)]).ToArray());
            ImgCaptcha.Source = new BitmapImage(new Uri("file:///D:/Teh/задание/Images/captcha_placeholder.png"));
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            UserEntity guest = new UserEntity
            {
                Id = 0,
                Surname = "Гость",
                FirstName = "",
                MiddleName = "",
                Login = "guest",
                Password = "",
                RoleId = 0
            };
            DashboardWindow dash = new DashboardWindow(guest);
            dash.Show();
            this.Close();
        }
    }
}
