using CinemaManagement.Entities;
using System;
using System.Collections.Generic;

public interface IMovieManager
{

    void AddMovie(string title, string director, string genre, string subtitle, int duration,
                  DateTime releaseDate, Dictionary<DateTime, List<(DateTime Start, DateTime End)>> showtimes, string status);

    bool RemoveMovie(string title, string director, string genre); // ✅ Cập nhật hàm xóa

    void RemoveAllMovies(); // ✅ Thêm mới

    void DisplayMovies();

    Movie GetMovieByTitle(string title, string director, string genre); // ✅ Cập nhật hàm tìm phim

    List<Movie> GetAllMovies();

    bool UpdateMovie(string title, string director, List<string> genres,
                 string newDirector, List<string> newGenres,
                 string newSubtitle, int newDuration, DateTime newReleaseDate,
                 Dictionary<DateTime, List<(DateTime Start, DateTime End)>> newShowtimes, string newStatus);

    void SaveMovies(); // ✅ Thêm mới

    // 📌 Thêm mới các hàm quản lý suất chiếu cho từng ngày
    bool AddShowtime(string title, DateTime date, (DateTime Start, DateTime End) showtime); // Thêm suất chiếu
    bool RemoveShowtime(string title, DateTime date, (DateTime Start, DateTime End) showtime); // Xóa suất chiếu
}
