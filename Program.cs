
using addaccount.Interface;
using CinemaManagement.Implementations;
using System;
using System.Globalization;
using System.Text;

namespace CinemaManagement
{
    class Program
    {
        static Dictionary<DateTime, List<(DateTime start, DateTime end)>> movieSchedule = new Dictionary<DateTime, List<(DateTime start, DateTime end)>>();

        static void Main(string[] args)
        {
           
            IUserManager userManager = new UserManager();
            IMovieManager movieManager = new MovieManager();
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n🎬 RẠP CHIẾU PHIM");
                Console.WriteLine("1. Đăng nhập");
                Console.WriteLine("2. Đăng kí");
                Console.WriteLine("3. Quên mật khẩu");
                Console.WriteLine("0. Thoát");
                Console.Write("👉 Nhập lựa chọn của bạn: ");
                string choice = Console.ReadLine();
                if (choice == "nimda")
                {
                    Console.WriteLine("🎭 Chế độ admin!");
                    while (true)
                    {
                        Console.WriteLine("\n🎬 ADMIN MENU");
                        Console.WriteLine("1. Quản lí danh sách phim");
                        Console.WriteLine("2. Quản lý tài khoản người dùng"); // ✅ Chức năng mới
                        Console.WriteLine("0. Đăng xuất");
                        Console.Write("👉 Chọn: ");
                        string adminChoice = Console.ReadLine();

                        if (adminChoice == "0")
                        {
                            break;
                        }
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

                                if (chon == "0")
                                {

                                    break;
                                }

                                else if (chon == "1") // Thêm phim
                                {
                                    Console.Write("Tên phim: ");
                                    string title = Console.ReadLine();
                                    Console.Write("Đạo diễn: ");
                                    string director = Console.ReadLine();

                                    List<string> genreList = new List<string>();
                                    Dictionary<string, string> genreDict = new Dictionary<string, string>
{
    {"1", "Hành động"}, {"2", "Phiêu lưu"}, {"3", "Khoa học viễn tưởng"},
    {"4", "Kinh dị"}, {"5", "Hài hước"}, {"6", "Lãng mạn"},
    {"7", "Hoạt hình"}, {"8", "Gia đình"}, {"9", "Tâm lý"},
    {"10", "Hình sự"}, {"11", "Âm nhạc"}, {"12", "Thần thoại"}, {"13", "Chính kịch"}
};

                                    while (true)
                                    {
                                        Console.WriteLine("\n📌 Chọn thể loại phim (có thể nhập nhiều thể loại, cách nhau bởi dấu phẩy, ví dụ: 1,2,5)");
                                        foreach (var item in genreDict)
                                        {
                                            Console.WriteLine($"{item.Key} - {item.Value}");
                                        }
                                        Console.Write("🔹 Nhập số tương ứng: ");

                                        string input = Console.ReadLine().Trim();

                                        if (string.IsNullOrEmpty(input))
                                        {
                                            Console.WriteLine("❌ Vui lòng nhập ít nhất một thể loại!");
                                            continue;
                                        }

                                        string[] inputs = input.Split(',');
                                        bool isValid = true;

                                        foreach (string value in inputs)
                                        {
                                            string trimmedValue = value.Trim();
                                            if (!genreDict.ContainsKey(trimmedValue))
                                            {
                                                Console.WriteLine($"❌ Lựa chọn không hợp lệ: {trimmedValue}. Vui lòng chỉ nhập các số từ 1 đến 13!");
                                                isValid = false;
                                                break;
                                            }

                                            if (!genreList.Contains(genreDict[trimmedValue]))
                                            {
                                                genreList.Add(genreDict[trimmedValue]);
                                            }
                                        }

                                        if (isValid)
                                        {
                                            break;
                                        }
                                    }
                                    string genreString = string.Join(", ", genreList); // Chuyển List<string> thành chuỗi
                                    Console.WriteLine($"✅ Bạn đã chọn thể loại: {genreString}");

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

                                    Dictionary<DateTime, List<(DateTime start, DateTime end)>> movieSchedule = new Dictionary<DateTime, List<(DateTime start, DateTime end)>>();

                                    while (true)
                                    {
                                        Console.Write("📅 Nhập các ngày suất chiếu (dd/MM/yyyy, dd/MM/yyyy, ...): ");
                                        string input = Console.ReadLine()?.Trim();
                                        string[] dateStrings = input.Split(',');

                                        bool isValid = true;
                                        List<DateTime> tempDates = new List<DateTime>();

                                        foreach (var dateString in dateStrings)
                                        {
                                            if (DateTime.TryParseExact(dateString.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime parsedDate))
                                            {
                                                tempDates.Add(parsedDate);
                                            }
                                            else
                                            {
                                                Console.WriteLine($"❌ Ngày không hợp lệ: {dateString.Trim()}! Hãy nhập theo định dạng dd/MM/yyyy.");
                                                isValid = false;
                                                break;
                                            }
                                        }

                                        if (isValid)
                                        {
                                            foreach (var date in tempDates)
                                            {
                                                movieSchedule[date] = new List<(DateTime start, DateTime end)>();
                                            }
                                            break;
                                        }
                                    }

                                    // ✅ Nhập suất chiếu cho từng ngày
                                    foreach (var date in movieSchedule.Keys.ToList())
                                    {
                                        Console.WriteLine($"\n📅 Nhập suất chiếu cho ngày {date:dd/MM/yyyy}:");

                                        while (true)
                                        {
                                            Console.Write("Nhập suất chiếu (HH:mm - HH:mm, HH:mm - HH:mm, ...): ");
                                            string input = Console.ReadLine()?.Trim();
                                            string[] sessions = input.Split(',');

                                            bool isValid = true;
                                            List<(DateTime start, DateTime end)> tempList = new List<(DateTime start, DateTime end)>();

                                            foreach (var session in sessions)
                                            {
                                                var parts = session.Trim().Split('-');

                                                if (parts.Length == 2 &&
                                                    DateTime.TryParseExact(parts[0].Trim(), "HH:mm", null, DateTimeStyles.None, out DateTime startTime) &&
                                                    DateTime.TryParseExact(parts[1].Trim(), "HH:mm", null, DateTimeStyles.None, out DateTime endTime))
                                                {
                                                    if (startTime.TimeOfDay < new TimeSpan(2, 0, 0))
                                                    {
                                                        Console.WriteLine("❌ Giờ bắt đầu phải từ 02:00 trở đi!");
                                                        isValid = false;
                                                        break;
                                                    }
                                                    else if (endTime.TimeOfDay > new TimeSpan(23, 59, 0))
                                                    {
                                                        Console.WriteLine("❌ Giờ kết thúc không được quá 23:59!");
                                                        isValid = false;
                                                        break;
                                                    }
                                                    else if (endTime <= startTime)
                                                    {
                                                        Console.WriteLine("❌ Giờ kết thúc phải lớn hơn giờ bắt đầu!");
                                                        isValid = false;
                                                        break;
                                                    }

                                                    DateTime fullStartTime = new DateTime(date.Year, date.Month, date.Day, startTime.Hour, startTime.Minute, 0);
                                                    DateTime fullEndTime = new DateTime(date.Year, date.Month, date.Day, endTime.Hour, endTime.Minute, 0);

                                                    // Kiểm tra chồng suất chiếu
                                                    if (movieSchedule[date].Any(s => (fullStartTime >= s.start && fullStartTime < s.end) || (fullEndTime > s.start && fullEndTime <= s.end)))
                                                    {
                                                        Console.WriteLine($"❌ Suất chiếu {fullStartTime:HH:mm} - {fullEndTime:HH:mm} bị chồng với suất khác!");
                                                        isValid = false;
                                                        break;
                                                    }

                                                    tempList.Add((fullStartTime, fullEndTime));
                                                }
                                                else
                                                {
                                                    Console.WriteLine("❌ Định dạng không hợp lệ! Hãy nhập theo định dạng HH:mm - HH:mm.");
                                                    isValid = false;
                                                    break;
                                                }
                                            }

                                            if (isValid)
                                            {
                                                movieSchedule[date].AddRange(tempList);
                                                break;
                                            }
                                        }
                                    }
                                    // ✅ Hiển thị danh sách suất chiếu
                                    Console.WriteLine("\n✅ Danh sách suất chiếu:");
                                    foreach (var entry in movieSchedule)
                                    {
                                        Console.WriteLine($"\n📅 Ngày {entry.Key:dd/MM/yyyy}:");
                                        foreach (var s in entry.Value)
                                        {
                                            Console.WriteLine($"🎥 {s.start:HH:mm} - {s.end:HH:mm}");
                                        }
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




                                    DateTime releaseDate = movieSchedule.Keys.Min();
                                    movieManager.AddMovie(title, director, genreString, subtitle, duration, releaseDate, movieSchedule, status);

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
                                        continue; // Thoát khỏi thao tác xóa
                                    }

                                    Console.Write("Nhập đạo diễn của phim: ");
                                    string director = Console.ReadLine();

                                    string genre;
                                    while (true)
                                    {
                                        Console.WriteLine("\n📌 Chọn thể loại phim (có thể nhập nhiều thể loại, cách nhau bằng dấu phẩy):");
                                        Console.WriteLine("1 - Hành động");
                                        Console.WriteLine("2 - Phiêu lưu");
                                        Console.WriteLine("3 - Khoa học viễn tưởng");
                                        Console.WriteLine("4 - Kinh dị");
                                        Console.WriteLine("5 - Hài hước");
                                        Console.WriteLine("6 - Lãng mạn");
                                        Console.WriteLine("7 - Hoạt hình");
                                        Console.WriteLine("8 - Gia đình");
                                        Console.WriteLine("9 - Tâm lý");
                                        Console.WriteLine("10 - Hình sự");
                                        Console.WriteLine("11 - Âm nhạc");
                                        Console.WriteLine("12 - Thần thoại");
                                        Console.WriteLine("13 - Chính kịch");
                                        Console.Write("🔹 Nhập số tương ứng: ");

                                        string input = Console.ReadLine().Trim();
                                        string[] inputs = input.Split(',');
                                        List<string> selectedGenres = new List<string>();

                                        Dictionary<string, string> genreDict = new Dictionary<string, string>
    {
        { "1", "Hành động" }, { "2", "Phiêu lưu" }, { "3", "Khoa học viễn tưởng" },
        { "4", "Kinh dị" }, { "5", "Hài hước" }, { "6", "Lãng mạn" },
        { "7", "Hoạt hình" }, { "8", "Gia đình" }, { "9", "Tâm lý" },
        { "10", "Hình sự" }, { "11", "Âm nhạc" }, { "12", "Thần thoại" }, { "13", "Chính kịch" }
    };

                                        bool validInput = true;
                                        foreach (var num in inputs)
                                        {
                                            if (genreDict.ContainsKey(num.Trim()))
                                            {
                                                selectedGenres.Add(genreDict[num.Trim()]);
                                            }
                                            else
                                            {
                                                validInput = false;
                                                break;
                                            }
                                        }

                                        if (!validInput)
                                        {
                                            Console.WriteLine("❌ Vui lòng nhập số từ 1 đến 13, phân cách bằng dấu phẩy!");
                                            continue;
                                        }

                                        genre = string.Join(", ", selectedGenres);
                                        break;
                                    }

                                    Console.WriteLine($"✅ Bạn đã chọn thể loại: {genre}");

                                    var movie = movieManager.GetMovieByTitle(title, director, genre);

                                    if (movie == null)
                                    {
                                        Console.WriteLine("⚠ Không tìm thấy phim!");
                                        continue;
                                    }

                                    Console.Write("\n❗ Bạn có chắc muốn xóa phim này không? (y/n): ");
                                    string confirmDelete = Console.ReadLine()?.Trim().ToLower();
                                    if (confirmDelete == "y")
                                    {
                                        movieManager.RemoveMovie(title, director, genre);
                                        Console.WriteLine("✅ Xóa phim thành công!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("❌ Hủy thao tác xóa phim.");
                                    }
                                }

                                else if (chon == "3")
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
                                    string genre;
                                    while (true)
                                    {
                                        Console.WriteLine("\n📌 Chọn thể loại phim (có thể nhập nhiều thể loại, cách nhau bằng dấu phẩy):");
                                        Console.WriteLine("1 - Hành động");
                                        Console.WriteLine("2 - Phiêu lưu");
                                        Console.WriteLine("3 - Khoa học viễn tưởng");
                                        Console.WriteLine("4 - Kinh dị");
                                        Console.WriteLine("5 - Hài hước");
                                        Console.WriteLine("6 - Lãng mạn");
                                        Console.WriteLine("7 - Hoạt hình");
                                        Console.WriteLine("8 - Gia đình");
                                        Console.WriteLine("9 - Tâm lý");
                                        Console.WriteLine("10 - Hình sự");
                                        Console.WriteLine("11 - Âm nhạc");
                                        Console.WriteLine("12 - Thần thoại");
                                        Console.WriteLine("13 - Chính kịch");
                                        Console.Write("🔹 Nhập số tương ứng: ");

                                        string input = Console.ReadLine().Trim();
                                        string[] inputs = input.Split(',');
                                        List<string> selectedGenres = new List<string>();

                                        Dictionary<string, string> genreDict = new Dictionary<string, string>
    {
        { "1", "Hành động" }, { "2", "Phiêu lưu" }, { "3", "Khoa học viễn tưởng" },
        { "4", "Kinh dị" }, { "5", "Hài hước" }, { "6", "Lãng mạn" },
        { "7", "Hoạt hình" }, { "8", "Gia đình" }, { "9", "Tâm lý" },
        { "10", "Hình sự" }, { "11", "Âm nhạc" }, { "12", "Thần thoại" }, { "13", "Chính kịch" }
    };

                                        bool validInput = true;
                                        foreach (var num in inputs)
                                        {
                                            if (genreDict.ContainsKey(num.Trim()))
                                            {
                                                selectedGenres.Add(genreDict[num.Trim()]);
                                            }
                                            else
                                            {
                                                validInput = false;
                                                break;
                                            }
                                        }

                                        if (!validInput)
                                        {
                                            Console.WriteLine("❌ Vui lòng nhập số từ 1 đến 13, phân cách bằng dấu phẩy!");
                                            continue;
                                        }

                                        genre = string.Join(", ", selectedGenres);
                                        break;
                                    }

                                    Console.WriteLine($"✅ Bạn đã chọn thể loại: {genre}");

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
                                        Console.WriteLine($"7. Thời gian suất chiếu: {string.Join(", ", movie.Showtimes.SelectMany(kvp => kvp.Value.Select(t => $"{t.Start:HH:mm} - {t.End:HH:mm}")))}");

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
                                            case "3": // ✅ Cập nhật thể loại phim
                                                Console.WriteLine("\n📌 Chọn thể loại mới (có thể nhập nhiều thể loại, cách nhau bằng dấu phẩy):");
                                                Console.WriteLine("1 - Hành động");
                                                Console.WriteLine("2 - Phiêu lưu");
                                                Console.WriteLine("3 - Khoa học viễn tưởng");
                                                Console.WriteLine("4 - Kinh dị");
                                                Console.WriteLine("5 - Hài hước");
                                                Console.WriteLine("6 - Lãng mạn");
                                                Console.WriteLine("7 - Hoạt hình");
                                                Console.WriteLine("8 - Gia đình");
                                                Console.WriteLine("9 - Tâm lý");
                                                Console.WriteLine("10 - Hình sự");
                                                Console.WriteLine("11 - Âm nhạc");
                                                Console.WriteLine("12 - Thần thoại");
                                                Console.WriteLine("13 - Chính kịch");
                                                Console.Write("🔹 Nhập số tương ứng: ");

                                                string genreInput = Console.ReadLine().Trim();
                                                string[] inputs = genreInput.Split(',');
                                                List<string> selectedGenres = new List<string>();

                                                Dictionary<string, string> genreDict = new Dictionary<string, string>
    {
        { "1", "Hành động" }, { "2", "Phiêu lưu" }, { "3", "Khoa học viễn tưởng" },
        { "4", "Kinh dị" }, { "5", "Hài hước" }, { "6", "Lãng mạn" },
        { "7", "Hoạt hình" }, { "8", "Gia đình" }, { "9", "Tâm lý" },
        { "10", "Hình sự" }, { "11", "Âm nhạc" }, { "12", "Thần thoại" }, { "13", "Chính kịch" }
    };

                                                bool validInput = true;
                                                foreach (var num in inputs)
                                                {
                                                    if (genreDict.ContainsKey(num.Trim()))
                                                    {
                                                        selectedGenres.Add(genreDict[num.Trim()]);
                                                    }
                                                    else
                                                    {
                                                        validInput = false;
                                                        break;
                                                    }
                                                }

                                                if (!validInput)
                                                {
                                                    Console.WriteLine("❌ Lựa chọn không hợp lệ! Thể loại không thay đổi.");
                                                }
                                                else
                                                {
                                                    movie.Genre = string.Join(", ", selectedGenres);
                                                    Console.WriteLine($"✅ Thể loại phim đã được cập nhật thành: {movie.Genre}");
                                                }
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
                                                while (true)
                                                {
                                                    Console.Write("📅 Nhập ngày phát hành mới (dd/MM/yyyy): ");
                                                    string inputDate = Console.ReadLine()?.Trim();

                                                    if (DateTime.TryParseExact(inputDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime newReleaseDate))
                                                    {
                                                        // ✅ Tính thứ trong tuần
                                                        string[] daysOfWeek = { "Chủ Nhật", "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy" };
                                                        string dayOfWeek = daysOfWeek[(int)newReleaseDate.DayOfWeek];

                                                        // ✅ Cập nhật và hiển thị thông tin mới
                                                        movie.ReleaseDate = newReleaseDate;
                                                        Console.WriteLine($"✅ Cập nhật thành công! Ngày phát hành mới: {dayOfWeek}, {newReleaseDate:dd/MM/yyyy}");
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("❌ Vui lòng nhập đúng định dạng ngày (dd/MM/yyyy)!");
                                                    }
                                                }
                                                break;
                                            case "7":
                                                Console.Write("📅 Nhập ngày muốn cập nhật suất chiếu (dd/MM/yyyy): ");
                                                string dateInput = Console.ReadLine()?.Trim();

                                                if (!DateTime.TryParseExact(dateInput, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime selectedDate))
                                                {
                                                    Console.WriteLine("❌ Ngày không hợp lệ! Hãy nhập theo định dạng dd/MM/yyyy.");
                                                    break;
                                                }

                                                if (!movieSchedule.ContainsKey(selectedDate))
                                                {
                                                    Console.WriteLine($"❌ Không tìm thấy suất chiếu cho ngày {selectedDate:dd/MM/yyyy}.");
                                                    break;
                                                }

                                                Console.Write("⏳ Nhập các suất chiếu mới (HH:mm - HH:mm, cách nhau bằng dấu phẩy): ");
                                                string input = Console.ReadLine()?.Trim();

                                                List<(DateTime start, DateTime end)> newShowtimes = new List<(DateTime start, DateTime end)>();

                                                var showtimePairs = input.Split(','); // Tách các suất chiếu

                                                foreach (var pair in showtimePairs)
                                                {
                                                    var parts = pair.Trim().Split('-');
                                                    if (parts.Length == 2 &&
                                                        DateTime.TryParseExact(parts[0].Trim(), "HH:mm", null, DateTimeStyles.None, out DateTime newStartTime) &&
                                                        DateTime.TryParseExact(parts[1].Trim(), "HH:mm", null, DateTimeStyles.None, out DateTime newEndTime))
                                                    {
                                                        // Kiểm tra thời gian hợp lệ
                                                        if (newStartTime.TimeOfDay < new TimeSpan(2, 0, 0) ||
                                                            newEndTime.TimeOfDay > new TimeSpan(23, 59, 0) ||
                                                            newEndTime <= newStartTime)
                                                        {
                                                            Console.WriteLine($"❌ Suất chiếu '{pair.Trim()}' không hợp lệ! (Phải từ 02:00 đến 23:59, EndTime > StartTime)");
                                                            continue;
                                                        }

                                                        DateTime fullStartTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, newStartTime.Hour, newStartTime.Minute, 0);
                                                        DateTime fullEndTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, newEndTime.Hour, newEndTime.Minute, 0);

                                                        // Kiểm tra chồng suất chiếu
                                                        if (newShowtimes.Any(s => (fullStartTime >= s.start && fullStartTime < s.end) ||
                                                                                  (fullEndTime > s.start && fullEndTime <= s.end)))
                                                        {
                                                            Console.WriteLine($"❌ Suất chiếu {fullStartTime:HH:mm} - {fullEndTime:HH:mm} bị chồng với suất khác!");
                                                            continue;
                                                        }

                                                        newShowtimes.Add((fullStartTime, fullEndTime));
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine($"❌ Định dạng sai ở '{pair.Trim()}'! Vui lòng nhập theo mẫu HH:mm - HH:mm.");
                                                    }
                                                }

                                                if (newShowtimes.Count > 0)
                                                {
                                                    movieSchedule[selectedDate].Clear(); // Xóa suất chiếu cũ
                                                    movieSchedule[selectedDate].AddRange(newShowtimes); // Thêm suất chiếu mới
                                                    Console.WriteLine("✅ Suất chiếu mới đã được cập nhật!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("❌ Không có suất chiếu hợp lệ nào được thêm!");
                                                }
                                                break;

                                            case "8": // ✅ Cập nhật trạng thái phim
                                                while (true)
                                                {
                                                    Console.WriteLine("\n📌 Chọn trạng thái mới:");
                                                    Console.WriteLine("1 - Đang chiếu");
                                                    Console.WriteLine("2 - Sắp chiếu");
                                                    Console.Write("🔹 Nhập số tương ứng: ");

                                                    string userInput = Console.ReadLine()?.Trim(); // ✅ Đổi tên biến

                                                    if (userInput == "1")
                                                    {
                                                        movie.Status = "Đang chiếu";
                                                        Console.WriteLine($"✅ Trạng thái phim đã được cập nhật thành: {movie.Status}");
                                                        break;
                                                    }
                                                    else if (userInput == "2")
                                                    {
                                                        movie.Status = "Sắp chiếu";
                                                        Console.WriteLine($"✅ Trạng thái phim đã được cập nhật thành: {movie.Status}");
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("❌ Lựa chọn không hợp lệ! Vui lòng nhập '1' hoặc '2'.\n");
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
                                Console.WriteLine("| STT | Username | Password | Email | Phone |");
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
                            while (true)
                            {
                                Console.WriteLine("Vui lòng nhập lựa chọn của bạn:");
                                Console.WriteLine("0. Thoát");
                                Console.WriteLine("1. Tìm và chọn phim");
                                Console.WriteLine("2. Quản lý tài khoản của tôi");
                                Console.Write("Lựa chọn của bạn: ");

                                string input = Console.ReadLine().Trim();

                                if (input == "0") break;


                                else if (input == "1")
                                {
                                    Console.WriteLine("🎬 Bạn đang ở chức năng 1: Tìm và chọn phim");
                                    Console.Write("Vui lòng nhập tên phim: ");
                                    string title = Console.ReadLine();
                                    Console.Write("Vui lòng nhập đạo diễn: ");
                                    string director = Console.ReadLine();
                                    string genre;

                                    while (true)
                                    {
                                        Console.WriteLine("\n📌 Chọn thể loại phim (có thể nhập nhiều thể loại, cách nhau bằng dấu phẩy):");
                                        Console.WriteLine("1 - Hành động, 2 - Phiêu lưu, 3 - Khoa học viễn tưởng, 4 - Kinh dị, 5 - Hài hước,");
                                        Console.WriteLine("6 - Lãng mạn, 7 - Hoạt hình, 8 - Gia đình, 9 - Tâm lý, 10 - Hình sự,");
                                        Console.WriteLine("11 - Âm nhạc, 12 - Thần thoại, 13 - Chính kịch");
                                        Console.Write("🔹 Nhập số tương ứng: ");

                                        string Uchon = Console.ReadLine().Trim();
                                        string[] inputs = Uchon.Split(',');
                                        List<string> selectedGenres = new List<string>();

                                        Dictionary<string, string> genreDict = new Dictionary<string, string>
        {
            { "1", "Hành động" }, { "2", "Phiêu lưu" }, { "3", "Khoa học viễn tưởng" },
            { "4", "Kinh dị" }, { "5", "Hài hước" }, { "6", "Lãng mạn" },
            { "7", "Hoạt hình" }, { "8", "Gia đình" }, { "9", "Tâm lý" },
            { "10", "Hình sự" }, { "11", "Âm nhạc" }, { "12", "Thần thoại" }, { "13", "Chính kịch" }
        };

                                        bool validInput = true;
                                        foreach (var num in inputs)
                                        {
                                            if (genreDict.ContainsKey(num.Trim()))
                                            {
                                                selectedGenres.Add(genreDict[num.Trim()]);
                                            }
                                            else
                                            {
                                                validInput = false;
                                                break;
                                            }
                                        }

                                        if (!validInput)
                                        {
                                            Console.WriteLine("❌ Vui lòng nhập số từ 1 đến 13, phân cách bằng dấu phẩy!");
                                            continue;
                                        }

                                        genre = string.Join(", ", selectedGenres);
                                        break;
                                    }

                                    Console.WriteLine($"✅ Bạn đã chọn thể loại: {genre}");

                                    var movie = movieManager.GetMovieByTitle(title, director, genre);

                                    if (movie == null)
                                    {
                                        Console.WriteLine("⚠ Không tìm thấy phim!");
                                        return;
                                    }

                                    Console.WriteLine("\n📌 Thông tin phim:");
                                    Console.WriteLine($"1. Tiêu đề: {movie.Title}");
                                    Console.WriteLine($"2. Đạo diễn: {movie.Director}");
                                    Console.WriteLine($"3. Thể loại: {movie.Genre}");
                                    Console.WriteLine($"4. Phụ đề: {movie.Subtitle}");
                                    Console.WriteLine($"5. Thời lượng: {movie.Duration} phút");
                                    Console.WriteLine($"6. Ngày phát hành: {movie.ReleaseDate:yyyy-MM-dd}");
                                    Console.WriteLine($"7. Trạng thái: {movie.Status}");

                                    // Hỏi người dùng có muốn đặt vé không
                                    Console.Write($"Bạn có muốn đặt vé xem phim {movie.Title} không? (y/n): ");
                                    string confirm = Console.ReadLine().ToLower();

                                    if (confirm == "y")
                                    {
                                        Console.WriteLine($"🎟 Bạn đã đặt vé xem phim {movie.Title} thành công!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("❌ Đặt vé đã bị hủy! Trở về màn hình chính.");
                                    }

                                    Console.WriteLine("🔹 Nhấn Enter để quay lại menu chính...");
                                    Console.ReadLine();
                                }






                                else if (input == "2")
                                {
                                    Console.WriteLine("👤 Bạn đang ở chức năng 2: Quản lý tài khoản");
                                    Console.WriteLine("Chức năng này đang phát triển vui lòng đợi sang bản cật nhật sau");
                                }
                                else
                                {
                                    Console.WriteLine("❌ Vui lòng nhập số 0, 1 hoặc 2!");
                                }
                            
                        }
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