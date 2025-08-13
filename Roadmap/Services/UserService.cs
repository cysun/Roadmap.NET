using Roadmap.Models;

namespace Roadmap.Services;

public class UserService(AppDbContext db)
{
    public User GetUser(string id) => db.Users.Find(id);

    public void AddUser(User user)
    {
        db.Users.Add(user);
        db.SaveChanges();
    }
}