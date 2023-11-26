# Cynnamon

A web api for cinema bookings and listings; using *latest* .NET 8 and Entity Framework Core. 

Inspire by reddit post: https://www.reddit.com/r/dotnet/comments/1841x0f/does_this_takehome_project_look_okay
as tbh, it looked like a fun project to do and I will be trying out some new tooling (Roselyn).

---
## Requirements
### MovieManagement
- [x] `Movie1`: Retrieve a list of available movies with their details (title, description, duration, genre, etc.).
- [x] `Movie2`: Add new movies to the system.
- [x] `Movie3`: Update existing movies.
- [ ] `Movie4`: Delete movies from the system.

### TheaterManagement
- [ ] `Theater1`: Create a new theater.
- [ ] `Theater2`: Update an existing theater.
- [ ] `Theater3`: Create a screening/showtime for a movie in a theater, including the date, time and available seats.
- [ ] `Theater4`: Cancel a screening/showtime for a movie in a theater.

### SeatReservation
- [ ] `Reservation1`: Allow users to reserve seats for a particular showtime.
- [ ] `Reservation2`: Ensure users can only reserve available seats.
- [ ] `Reservation3`: Implement a reservation timeout to release unconfirmed/unpaid seats after a set amount of time.

### BookingConfirmation
- [ ] `Confirmation1`: Allow users to confirm their reservation.
- [ ] `Confirmation2`: Provide booking details, including the movie, showtime, seats and price.
