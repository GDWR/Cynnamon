using System.Text.Json.Serialization;

namespace Cynnamon.Models;

public class Movie(int? id, string title, string description, string duration, string genre) {
    public int? Id { get; set; } = id;
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
    public string Duration { get; set; } = duration;
    public string Genre { get; set; } = genre;

    [JsonIgnore] public bool Deleted { get; set; } = false;
}
