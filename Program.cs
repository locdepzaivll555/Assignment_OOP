
using addaccount.Interface;
using CinemaManagement.Implementations;
using System;

namespace CinemaManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            IUserManager userManager = new UserManager();
            IMovieManager movieManager = new MovieManager();
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n🎬 RẠP CHIẾU PHIM");
                Console.WriteLine("1. Sign in");
                Console.WriteLine("2. Sign up");
                Console.WriteLine("3. Forgot password");
                Console.WriteLine("0. Exit");
                Console.Write("👉 Input your choice: ");
                string choice = Console.ReadLine();
                if (choice == "nimda")
                {
                    Console.WriteLine("🎭 Admin mode activated!");
                    while (true)
                    {
                        Console.WriteLine("\n🎬 ADMIN MENU");
                        Console.WriteLine("1. Quan ly danh sach phim");
                        Console.WriteLine("2. Quản lý tài khoản người dùng"); // ✅ Chức năng mới
                        Console.WriteLine("0. Đăng xuất");
                        Console.Write("👉 Chọn: ");
                        string adminChoice = Console.ReadLine();

                        if (adminChoice == "0") break;
                        else if (adminChoice == "1")
                        {
                            movieManager.DisplayMovies();
                            // Hiển thị danh sách phim trước

                            while (true)
                            {
                                Console.WriteLine("\nVui lòng nhập lựa chọn của bạn:");
                                Console.WriteLine("0. Trở về");
                                Console.WriteLine("1. Thêm phim");
                                Console.WriteLine("2. Xóa phim");
                                Console.WriteLine("3. Sửa phim");

                                Console.Write("Lựa chọn: ");
                                string chon = Console.ReadLine();

                                if (chon == "0") break;

                                else if (chon == "1") // Thêm phim
                                {
                                    Console.Write("Tên phim: ");
                                    string title = Console.ReadLine();
                                    Console.Write("Đạo diễn: ");
                                    string director = Console.ReadLine();
                                    Console.Write("Thể loại: ");
                                    string genre = Console.ReadLine();
                                    Console.Write("Phụ đề: ");
                                    string subtitle = Console.ReadLine();

                                    int duration;
                                    while (true)
                                    {
                                        Console.Write("Thời lượng (phút): ");
                                        if (int.TryParse(Console.ReadLine(), out duration) && duration > 0)
                                            break;
                                        Console.WriteLine("❌ Vui lòng nhập số nguyên hợp lệ (lớn hơn 0)!");
                                    }

                                    DateTime releaseDate;
                                    while (true)
                                    {
                                        Console.Write("Ngày phát hành (yyyy-MM-dd): ");
                                        if (DateTime.TryParse(Console.ReadLine(), out releaseDate))
                                            break;
                                        Console.WriteLine("❌ Vui lòng nhập đúng định dạng ngày (yyyy-MM-dd)!");
                                    }

                                    DateTime showTime;
                                    while (true)
                                    {
                                        Console.Write("Giờ chiếu (HH:mm): ");
                                        if (DateTime.TryParse(Console.ReadLine(), out showTime))
                                            break;
                                        Console.WriteLine("❌ Vui lòng nhập đúng định dạng giờ (HH:mm)!");
                                    }

                                    string status;
                                    while (true)
                                    {
                                        Console.Write("Trạng thái (1 = Đang chiếu, 2 = Sắp chiếu): ");
                                        string input = Console.ReadLine().Trim();

                                        if (input == "1")
                                        {
                                            status = "Đang chiếu";
                                            break;
                                        }
                                        else if (input == "2")
                                        {
                                            status = "Sắp chiếu";
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("❌ Vui lòng nhập số 1 hoặc 2!");
                                        }
                                    }

                                    movieManager.AddMovie(title, director, genre, subtitle, duration, releaseDate, showTime, status);
                                    Console.WriteLine("✅ Đã thêm phim thành công!");
                                }

                                else if (chon == "2") // Xóa phim
                                {
                                    var movies = movieManager.GetAllMovies();
                                    if (movies == null || movies.Count == 0)
                                    {
                                        Console.WriteLine("⚠ Danh sách trống, không thể xóa!");
                                        continue; // Quay lại menu quản lý phim mà không thoát
                                    }

                                    movieManager.DisplayMovies();

                                    Console.Write("Nhập tên phim cần xóa: ");
                                    string title = Console.ReadLine();
                                    Console.Write("Nhập đạo diễn của phim: ");
                                    string director = Console.ReadLine();
                                    Console.Write("Nhập thể loại của phim: ");
                                    string genre = Console.ReadLine();

                                    if (title == "DELETE_ALL")
                                    {
                                        Console.Write("❗ Bạn có chắc muốn xóa toàn bộ danh sách phim không? (y/n): ");
                                        string confirm = Console.ReadLine()?.Trim().ToLower();
                                        if (confirm == "y")
                                        {
                                            movieManager.RemoveAllMovies();
                                            Console.WriteLine("✅ Đã xóa toàn bộ phim!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("✅ Hủy thao tác xóa toàn bộ phim.");
                                        }
                                    }
                                    else
                                    {
                                        var movie = movieManager.GetMovieByTitle(title, director, genre);
                                        if (movie == null)
                                        {
                                            Console.WriteLine("⚠ Không tìm thấy phim!");
                                            continue;
                                        }

                                        Console.Write("\n❗ Bạn có chắc muốn xóa phim này không? (y/n): ");
                                        string confirm = Console.ReadLine()?.Trim().ToLower();
                                        if (confirm == "y")
                                        {
                                            movieManager.RemoveMovie(title, director, genre);
                                            Console.WriteLine("✅ Xóa phim thành công!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("❌ Hủy thao tác xóa phim.");
                                        }
                                    }
                                }


                                else if (chon == "3" )
                                {

                                    var movies = movieManager.GetAllMovies();
                                    if (movies == null || movies.Count == 0)
                                    {
                                        Console.WriteLine("⚠ Danh sách trống, không thể xóa!");
                                        continue; // Quay lại menu quản lý phim mà không thoát
                                    }
                                    movieManager.DisplayMovies();

                                    Console.Write("Vui lòng nhập tên phim: ");
                                    string title = Console.ReadLine();
                                    Console.Write("Vui lòng nhập đạo diễn: ");
                                    string director = Console.ReadLine();
                                    Console.Write("Vui lòng nhập thể loại: ");
                                    string genre = Console.ReadLine();

                                    var movie = movieManager.GetMovieByTitle(title, director, genre);
                                    if (movie == null)
                                    {
                                        Console.WriteLine("⚠ Không tìm thấy phim!");
                                        continue;
                                    }

                                    while (true)
                                    {
                                        Console.WriteLine("\n📌 Thông tin phim:");
                                        Console.WriteLine($"1. Tiêu đề: {movie.Title}");
                                        Console.WriteLine($"2. Đạo diễn: {movie.Director}");
                                        Console.WriteLine($"3. Thể loại: {movie.Genre}");
                                        Console.WriteLine($"4. Phụ đề: {movie.Subtitle}");
                                        Console.WriteLine($"5. Thời lượng: {movie.Duration} phút");
                                        Console.WriteLine($"6. Ngày phát hành: {movie.ReleaseDate:yyyy-MM-dd}");
                                        Console.WriteLine($"7. Giờ chiếu: {movie.ShowTime:HH:mm}");
                                        Console.WriteLine($"8. Trạng thái: {movie.Status}");
                                        Console.WriteLine("0. Trở về");

                                        Console.Write("Vui lòng nhập phần muốn sửa: ");
                                        string chonSua = Console.ReadLine();

                                        if (chonSua == "0") break;

                                        switch (chonSua)
                                        {
                                            case "1":
                                                Console.Write("Nhập tiêu đề mới: ");
                                                movie.Title = Console.ReadLine();
                                                break;
                                            case "2":
                                                Console.Write("Nhập đạo diễn mới: ");
                                                movie.Director = Console.ReadLine();
                                                break;
                                            case "3":
                                                Console.Write("Nhập thể loại mới: ");
                                                movie.Genre = Console.ReadLine();
                                                break;
                                            case "4":
                                                Console.Write("Nhập phụ đề mới: ");
                                                movie.Subtitle = Console.ReadLine();
                                                break;
                                            case "5":
                                                Console.Write("Nhập thời lượng mới (phút): ");
                                                if (int.TryParse(Console.ReadLine(), out int newDuration))
                                                    movie.Duration = newDuration;
                                                else
                                                    Console.WriteLine("❌ Vui lòng nhập số nguyên hợp lệ!");
                                                break;
                                            case "6":
                                                Console.Write("Nhập ngày phát hành mới (dd/MM/yyyy): ");
                                                if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime newReleaseDate))
                                                    movie.ReleaseDate = newReleaseDate;
                                                else
                                                    Console.WriteLine("❌ Vui lòng nhập đúng định dạng ngày (dd/MM/yyyy)!");
                                                break;

                                            case "7":
                                                Console.Write("Nhập giờ chiếu mới (HH:mm): ");
                                                if (DateTime.TryParse(Console.ReadLine(), out DateTime newShowTime))
                                                    movie.ShowTime = newShowTime;
                                                else
                                                    Console.WriteLine("❌ Vui lòng nhập đúng định dạng giờ!");
                                                break;
                                            case "8":
                                                while (true)
                                                {
                                                    Console.Write("Trạng thái mới (1 = Đang chiếu, 2 = Sắp chiếu): ");
                                                    string input = Console.ReadLine().Trim();

                                                    if (input == "1")
                                                    {
                                                        movie.Status = "Đang chiếu";
                                                        break;
                                                    }
                                                    else if (input == "2")
                                                    {
                                                        movie.Status = "Sắp chiếu";
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("❌ Vui lòng nhập số 1 hoặc 2!");
                                                    }
                                                }
                                                break;
                                            default:
                                                Console.WriteLine("⚠ Lựa chọn không hợp lệ!");
                                                break;
                                        }
                                    }

                                    Console.Write("📢 Bạn có muốn lưu thay đổi này không? (y/n): ");
                                    string confirmSave = Console.ReadLine()?.Trim().ToLower();
                                    if (confirmSave == "y")
                                    {
                                        movieManager.SaveMovies();
                                        Console.WriteLine("✅ Cập nhật thông tin phim thành công!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("❌ Bạn đã hủy lưu thay đổi.");
                                    }
                                }

                            }
                        }

                        else if (adminChoice == "2") // ✅ Chức năng mới: Quản lý tài khoản người dùng
                        {
                            while (true)
                            {
                                Console.WriteLine("\n👥 DANH SÁCH NGƯỜI DÙNG");
                                Console.WriteLine("| No | Username | Password | Email | Phone |");
                                Console.WriteLine("---------------------------------------------------");

                                string[] accounts = File.ReadAllLines("account.txt");

                                for (int i = 0; i < accounts.Length; i++)
                                {
                                    string[] userInfo = accounts[i].Split('|');
                                    if (userInfo.Length == 4) // Đảm bảo dữ liệu hợp lệ
                                    {
                                        Console.WriteLine($"| {i + 1}  | {userInfo[0]} | {userInfo[1]} | {userInfo[2]} | {userInfo[3]} |");
                                    }
                                }

                                Console.Write("\nVui lòng nhập username để xóa (hoặc nhập 'DELETE_ALL' để xóa toàn bộ tài khoản, hoặc ấn 0 để thoát): ");
                                string usernameToDelete = Console.ReadLine();

                                if (usernameToDelete == "0") break;

                                if (usernameToDelete == "DELETE_ALL")
                                {
                                    while (true)
                                    {
                                        Console.Write("⚠️ Bạn có chắc muốn xóa toàn bộ tài khoản không? (y/n): ");
                                        string confirm = Console.ReadLine().Trim().ToLower();

                                        // Chỉ chấp nhận 'y' hoặc 'n'
                                        if (confirm == "y")
                                        {
                                            File.WriteAllText("account.txt", ""); // Xóa toàn bộ nội dung file
                                            Console.WriteLine("✅ Tất cả tài khoản đã bị xóa!");
                                            break;
                                        }
                                        else if (confirm == "n")
                                        {
                                            Console.WriteLine("❌ Hủy thao tác xóa tất cả tài khoản. Quay lại menu.");
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("❌ Chỉ được nhập 'y' (Yes) hoặc 'n' (No). Vui lòng nhập lại!");
                                        }
                                    }
                                    continue;
                                }

                                bool userFound = false;
                                List<string> updatedAccounts = new List<string>();

                                foreach (string account in accounts)
                                {
                                    string[] userInfo = account.Split('|');
                                    if (userInfo.Length == 4 && userInfo[0] == usernameToDelete)
                                    {
                                        userFound = true;

                                        while (true)
                                        {
                                            Console.Write($"⚠️ Bạn có chắc muốn xóa tài khoản '{usernameToDelete}' không? (y/n): ");
                                            string confirm = Console.ReadLine().Trim().ToLower();

                                            if (confirm == "y")
                                            {
                                                Console.WriteLine($"✅ Đã xóa tài khoản: {usernameToDelete}");
                                                break; // Bỏ qua tài khoản này khi ghi lại file
                                            }
                                            else if (confirm == "n")
                                            {
                                                Console.WriteLine("❌ Hủy thao tác xóa tài khoản.");
                                                updatedAccounts.Add(account); // Giữ lại tài khoản này
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("❌ Chỉ được nhập 'y' (Yes) hoặc 'n' (No). Vui lòng nhập lại!");
                                            }
                                        }
                                        continue;
                                    }
                                    updatedAccounts.Add(account);
                                }

                                if (!userFound)
                                    Console.WriteLine("❌ Không tìm thấy username này!");
                                else
                                    File.WriteAllLines("account.txt", updatedAccounts);
                            }
                        }

                        else
                        {
                            Console.WriteLine("❌ Lựa chọn không hợp lệ!");
                        }
                    }
                    continue; // Quay lại vòng lặp chính để tránh xử lý tiếp phần dưới
                }

                // Tiếp tục kiểm tra các lựa chọn số
                if (int.TryParse(choice, out int numChoice))
                {
                    if (numChoice == 0) break;
                    else if (numChoice == 1)
                    {
                        Console.Write("🔑 Username or Email: ");
                        string usernameOrEmail = Console.ReadLine();
                        Console.Write("🔑 Password: ");
                        string password = ReadPassword();

                        if (userManager.SignIn(usernameOrEmail, password))
                        {
                            Console.WriteLine("✅ Đăng nhập thành công!");
                            movieManager.DisplayMovies();
                        }
                        else Console.WriteLine("❌ Sai tài khoản hoặc mật khẩu!");
                    }
                    else if (numChoice == 2)
                    {
                        userManager.SignUp(); // Gọi trực tiếp mà không truyền tham số
                    }

                    else if (numChoice == 3)
                    {
                        Console.Write("📞 Nhập số điện thoại của bạn: ");
                        string phone = Console.ReadLine();

                        string recoveredPassword = userManager.RecoverPassword(phone);
                        if (recoveredPassword != null)
                            Console.WriteLine($"🔑 Mật khẩu của bạn là: {recoveredPassword}");
                        else
                            Console.WriteLine("❌ Không tìm thấy số điện thoại!");
                    }
                    else
                    {
                        Console.WriteLine("❌ Lựa chọn không hợp lệ!");
                    }
                }
                else
                {
                    Console.WriteLine("❌ Lựa chọn không hợp lệ!");
                }
            }

            // 🔹 Hàm nhập password ẩn
            static string ReadPassword()
            {
                string password = "";
                ConsoleKeyInfo key;

                do
                {
                    key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Enter) break;
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        if (password.Length > 0)
                        {
                            password = password[..^1]; // Xóa ký tự cuối
                            Console.Write("\b \b"); // Xóa dấu '*' trên màn hình
                        }
                    }
                    else
                    {
                        password += key.KeyChar;
                        Console.Write("*");
                    }

                } while (true);

                Console.WriteLine();
                return password;
            }

        }
    }
}