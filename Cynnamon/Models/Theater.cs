namespace Cynnamon.Models;

public class Theater(int? id, string name, string location, int seats) {
    public int? Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Location { get; set; } = location;
    public int Seats { get; set; } = seats;
}
