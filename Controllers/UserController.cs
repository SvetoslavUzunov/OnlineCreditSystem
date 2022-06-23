namespace OnlineCreditSystem.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using OnlineCreditSystem.Data;
using OnlineCreditSystem.Data.Models;
using OnlineCreditSystem.Models.Transaction;
using OnlineCreditSystem.Models.User;
using static GlobalConstants;

public class UserController : Controller
{
    private readonly OnlineCreditSystemDbContext data;
    private const int creditsForRegistration = 100;

    public UserController(OnlineCreditSystemDbContext data)
        => this.data = data;

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var userData = new User
        {
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Credits = 0
        };

        var hashPassword = ComputeSha256Hash(user.Password);
        userData.Password = hashPassword;
        userData.ConfirmPassword = hashPassword;

        userData.Credits += creditsForRegistration;

        data.Users.Add(userData);
        data.SaveChanges();

        TempData[GlobalMessageKey] = "Successfully registration! Тo use the full features of the site, please login.";

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(User user)
    {
        var hashPassword = ComputeSha256Hash(user.Password);

        var account = data.Users
            .FirstOrDefault(u => u.Email == user.Email && u.Password == hashPassword);

        if (account == null)
        {
            ModelState.AddModelError("", "Email or password are wrong!");

            return View();
        }

        HttpContext.Session.SetString("UserID", account.Id.ToString());
        HttpContext.Session.SetString("Email", account.Email);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Home");
    }

    public IActionResult MyAccount(int id)
    {
        var userData = data.Users
            .Where(u => u.Id == id)
            .Select(u => new UserViewModel
            {
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Credits = u.Credits
            })
            .FirstOrDefault();

        return View(userData);
    }

    public IActionResult MakeTransaction()
    {
        return View();
    }

    [HttpPost]
    public IActionResult MakeTransaction(UserTransactionFormModel transaction, int userId)
    {
        var currentUser = data.Users.Find(userId);

        if (currentUser == null)
        {
            return View();
        }

        if (currentUser.Credits < transaction.AmountCredits)
        {
            return View();
        }

        currentUser.Credits -= transaction.AmountCredits;

        var targetUser = data.Users
            .FirstOrDefault(u => (u.PhoneNumber == transaction.PhoneNumber) && (u.Email != currentUser.Email));

        if (targetUser == null)
        {
            ModelState.AddModelError(nameof(targetUser), "User not found!");

            return View();
        }

        targetUser.Credits += transaction.AmountCredits;

        var transactionData = new Transaction
        {
            AmountCredits = transaction.AmountCredits,
            UserId = currentUser.Id,
            Comment = transaction.Comment ?? "No comment",
            Date = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")
        };

        data.Transactions.Add(transactionData);
        data.SaveChanges();

        TempData[GlobalMessageKey] = "Transaction was successfully!";

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Dashboard(int userId)
    {
        var user = data.Users.Find(userId);
        if (user != null)
        {
            var userTransactions = data.Transactions
                .Where(t => t.UserId == user.Id)
                .Select(t => new TransactionViewModel
                {
                    Id = t.Id,
                    AmountCredits = t.AmountCredits,
                    Comment = t.Comment ?? "No comment",
                    User = t.User,
                    Date = t.Date
                })
                .ToList();

            return View(userTransactions);
        }

        return View();
    }

    public IActionResult AllUsers()
    {
        var users = data.Users.Select(u => new UserViewModel
        {
            Id = u.Id,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Credits = u.Credits
        })
        .ToList();

        return View(users);
    }

    public IActionResult AdminDashboard()
    {
        var transtactions = data.Transactions
            .Select(t => new TransactionViewModel
            {
                Id = t.Id,
                AmountCredits = t.AmountCredits,
                Comment = t.Comment,
                User = t.User,
                Date = t.Date
            })
            .ToList();

        return View(transtactions);
    }

    private static string ComputeSha256Hash(string rawData)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

        StringBuilder builder = new();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }
}
