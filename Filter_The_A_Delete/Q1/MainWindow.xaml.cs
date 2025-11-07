using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Q1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Danh sách các Star đã thêm
        private List<Star> stars = new List<Star>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAddToList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(txtStarName.Text) || dpDob.SelectedDate == null)
                {
                    MessageBox.Show("Please enter Star Name and Date of Birth.");
                    return;
                }

                Star star = new Star
                {
                    Name = txtStarName.Text,
                    Dob = dpDob.SelectedDate.Value,
                    Description = txtDescription.Text,
                    Male = chkIsMale.IsChecked,
                    Nationality = txtNationality.Text
                };

                // Thêm vào danh sách
                stars.Add(star);

                // Hiển thị danh sách ra ô TextBox bên phải
                RefreshStarListDisplay();

                // Xóa input để tiện nhập mới
                txtStarName.Clear();
                txtDescription.Clear();
                txtNationality.Clear();
                chkIsMale.IsChecked = false;
                dpDob.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding star: {ex.Message}");
            }   
        }

        private void RefreshStarListDisplay()
        {
            txtList.Clear();
            foreach (var s in stars)
            {
                txtList.AppendText($"{s.Name}\t{s.Dob.ToString()}\t{s.Description}\t{s.Male}\t{s.Nationality}\n");
            }
        }

        private async void btnSendToServer_Click(object sender, RoutedEventArgs e)
        {
            if (stars.Count == 0)
            {
                MessageBox.Show("No star data to send.");
                return;
            }

            try
            {
                string json = JsonSerializer.Serialize(stars);
                using (TcpClient client = new TcpClient("127.0.0.1", 5000))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(json);
                    await stream.WriteAsync(data, 0, data.Length);

                    // Nhận phản hồi từ server
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

                    if (response.Equals("accepted", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("✅ Data accepted by server!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("❌ Server rejected data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error: {ex.Message}");
            }
        }
    }
}