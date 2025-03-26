using System;
using System.IO;
using System.Linq;
using addaccount.Interface;
using CinemaManagement.Entities;

namespace CinemaManagement.Implementations
{
    public class UserManager : IUserManager
    {
        private const string FilePath = "account.txt";
        public void SignUp() // Không có tham số
{
    string username, email, phone, password;

    // ✅ Kiểm tra Username chặt chẽ
    while (true)
    {
        Console.Write("🔹 Nhập username (8-12 ký tự): ");
        username = Console.ReadLine()?.Trim() ?? "";

        if (!IsValidLength(username))
        {
            Console.WriteLine("❌ Username phải từ 8 đến 12 ký tự! Hãy nhập lại.");
            continue;
        }

        if (!IsUniqueUser(username, null, null))
        {
            Console.WriteLine("❌ Username đã tồn tại! Hãy nhập lại.");
            continue;
        }

        break;
    }

            // ✅ Kiểm tra Password
            // ✅ Kiểm tra Password chặt chẽ
            while (true)
            {
                Console.Write("🔹 Nhập password (8-12 ký tự): ");
                password = Console.ReadLine()?.Trim() ?? "";

                if (!IsValidLength(password))
                {
                    Console.WriteLine("❌ Password phải từ 8 đến 12 ký tự! Hãy nhập lại.");
                    continue;
                }

                Console.Write("🔹 Xác nhận lại password: ");
                string confirmPassword = Console.ReadLine()?.Trim() ?? "";

                if (password != confirmPassword)
                {
                    Console.WriteLine("❌ Password không khớp! Hãy nhập lại.");
                    continue;
                }

                break;
            }


            // ✅ Kiểm tra Email
            while (true)
    {
        Console.Write("🔹 Nhập email (@gmail.com): ");
        email = Console.ReadLine()?.Trim() ?? "";

        if (!IsValidGmail(email))
        {
            Console.WriteLine("❌ Email không hợp lệ! Hãy nhập lại.");
            continue;
        }

        if (!IsUniqueUser(null, email, null))
        {
            Console.WriteLine("❌ Email đã tồn tại! Hãy nhập lại.");
            continue;
        }

        break;
    }

    // ✅ Kiểm tra Số điện thoại
    while (true)
    {
        Console.Write("🔹 Nhập số điện thoại (10 số, bắt đầu bằng 0): ");
        phone = Console.ReadLine()?.Trim() ?? "";

        if (!IsValidPhoneNumber(phone))
        {
            Console.WriteLine("❌ Số điện thoại không hợp lệ! Hãy nhập lại.");
            continue;
        }

        if (!IsUniqueUser(null, null, phone))
        {
            Console.WriteLine("❌ Số điện thoại đã tồn tại! Hãy nhập lại.");
            continue;
        }

        break;
    }

    // ✅ Lưu vào file
    File.AppendAllText("account.txt", $"{username}|{password}|{email}|{phone}\n");
    Console.WriteLine("✅ Đăng ký thành công!");



            // ✅ Quay về màn hình chính
            Console.WriteLine("🔄 Đang trở về màn hình chính...\n");
        }
        public bool SignIn(string usernameOrEmail, string password)
        {
            if (!File.Exists(FilePath)) return false;

            string[] accounts = File.ReadAllLines(FilePath);
            return accounts.Any(account =>
            {
                string[] data = account.Split('|'); // Cập nhật dấu phân tách
                return data.Length >= 4 &&
                       (data[0] == usernameOrEmail || data[2] == usernameOrEmail) &&
                       data[1] == password;
            });
        }

        private bool IsUniqueUser(string username, string email, string phone)
        {
            if (!File.Exists(FilePath)) return true;

            return !File.ReadAllLines(FilePath).Any(line =>
            {
                string[] data = line.Split('|'); // Cập nhật dấu phân tách

                // Kiểm tra xem dòng có đủ 4 phần tử không
                if (data.Length < 4) return false;

                return (username != null && data[0] == username) ||
                       (email != null && data[2] == email) ||
                       (phone != null && data[3] == phone);
            });
        }
        public string RecoverPassword(string phone)
        {
            if (!File.Exists("account.txt"))
                return null;

            string[] lines = File.ReadAllLines("account.txt");

            foreach (string line in lines)
            {
                string[] parts = line.Split('|'); // Cập nhật dấu phân tách
                if (parts.Length >= 4 && parts[3] == phone) // Số điện thoại nằm ở vị trí thứ 4
                {
                    return parts[1]; // Trả về mật khẩu
                }
            }

            return null; // Không tìm thấy
        }

        public bool VerifyPassword(string username, string password)
        {
            if (!File.Exists(FilePath)) return false;

            string[] accounts = File.ReadAllLines(FilePath);
            return accounts.Any(account =>
            {
                string[] data = account.Split('|');
                return data.Length >= 4 && data[0] == username && data[1] == password;
            });
        }

        public void UpdatePassword(string username, string newPassword)
        {
            if (!File.Exists(FilePath)) return;

            var lines = File.ReadAllLines(FilePath).ToList();
            for (int i = 0; i < lines.Count; i++)
            {
                string[] data = lines[i].Split('|');
                if (data.Length >= 4 && data[0] == username)
                {
                    data[1] = newPassword;
                    lines[i] = string.Join("|", data);
                    break;
                }
            }
            File.WriteAllLines(FilePath, lines);
        }

        public void DeleteAccount(string username)
        {
            if (!File.Exists(FilePath)) return;

            var lines = File.ReadAllLines(FilePath).Where(line => !line.StartsWith(username + "|")).ToList();
            File.WriteAllLines(FilePath, lines);
        }


        private bool IsValidPhoneNumber(string phone)
        {
            return phone.Length == 10 && phone.StartsWith("0") && phone.All(char.IsDigit);
        }

        private bool IsValidLength(string input)
        {
            return !string.IsNullOrEmpty(input) && input.Length >= 8 && input.Length <= 12;
        }

        private bool IsValidGmail(string email)
        {
            return email.EndsWith("@gmail.com") && email.Length > 10; // Đảm bảo có ít nhất 1 ký tự trước @
        }
    }
}
