using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MyEvent;

public class Helper
{
    private readonly IWebHostEnvironment en;
    private readonly IHttpContextAccessor ct;

    // TODO
    public Helper(IWebHostEnvironment en, IHttpContextAccessor ct)
    {
        this.en = en;
        this.ct = ct;
    }

    // ------------------------------------------------------------------------
    // Photo Upload
    // ------------------------------------------------------------------------

    public string ValidatePhoto(IFormFile f)
    {
        var reType = new Regex(@"^image\/(jpeg|png)$", RegexOptions.IgnoreCase);
        var reName = new Regex(@"^.+\.(jpeg|jpg|png)$", RegexOptions.IgnoreCase);

        if (!reType.IsMatch(f.ContentType) || !reName.IsMatch(f.FileName))
        {
            return "Only JPG and PNG photo is allowed.";
        }
        else if (f.Length > 1 * 1024 * 1024)
        {
            return "Photo size cannot more than 1MB.";
        }

        return "";
    }

    public string SavePhoto(IFormFile f, string folder)
    {
        // Save inside wwwroot/images/{folder}
        var saveFolder = Path.Combine(en.WebRootPath, "images", folder);
        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);

        // Generate unique file name
        var fileName = Guid.NewGuid().ToString("n") + Path.GetExtension(f.FileName);
        var filePath = Path.Combine(saveFolder, fileName);

        // Resize options
        var options = new ResizeOptions
        {
            Size = new Size(200, 500),
            Mode = ResizeMode.Crop,
        };

        using var stream = f.OpenReadStream();
        using var img = Image.Load(stream);
        img.Mutate(x => x.Resize(options));
        img.Save(filePath);

        // Return relative web path (for <img src>)
        return $"/images/{folder}/{fileName}";
    }
    

    public void DeletePhoto(string file, string folder)
    {
        file = Path.GetFileName(file);
        var path = Path.Combine(en.WebRootPath, folder, file);
        File.Delete(path);
    }



    // ------------------------------------------------------------------------
    // Security Helper Functions
    // ------------------------------------------------------------------------

    // TODO
    private readonly PasswordHasher<object> ph = new();

    public string HashPassword(string password)
    {
        return ph.HashPassword(0, password); 
    }

    public bool VerifyPassword(string hash, string password)
    {
        return ph.VerifyHashedPassword(0, hash, password) == PasswordVerificationResult.Success; 
    }

    public void SignIn(HttpContext httpContext, int userId, string name, string email, string role, bool rememberMe)
    {
        var claims = new List<Claim>
    {
        new Claim("UserId", userId.ToString()),
        new Claim(ClaimTypes.Name, name),
        new Claim(ClaimTypes.Email, email),
        new Claim(ClaimTypes.Role, role)
    };

        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

         httpContext.SignInAsync("Cookies", claimsPrincipal, new AuthenticationProperties
        {
            IsPersistent = rememberMe,
            ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
        });
    }



    public void SignOut()
    {
        // Sign out
        ct.HttpContext!.SignOutAsync();
        // TODO
    }

    public string RandomPassword()
    {
        string s = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string password = "";

        // TODO
        Random r = new();

        for (int i = 1; i <= 10; i++)
        {
            password += s[r.Next(s.Length)];
        }

        return password;
    }
}
