using CinemaManagement.Entities;

public interface IMovieManager
{
    void AddMovie(string title, string director, string genre, string subtitle, int duration, DateTime releaseDate, DateTime showTime, string status);
    bool RemoveMovie(string title, string director, string genre); // ✅ Cập nhật hàm xóa
    void RemoveAllMovies(); // ✅ Thêm mới
    void DisplayMovies();
    Movie GetMovieByTitle(string title, string director, string genre); // ✅ Cập nhật hàm tìm phim
    List<Movie> GetAllMovies();
    bool UpdateMovie(string title, string newDirector, string newGenre, string newSubtitle, int newDuration, DateTime newReleaseDate, DateTime newShowTime, string newStatus);
    void SaveMovies(); // ✅ Thêm mới
    //bool HasMovies();
}

