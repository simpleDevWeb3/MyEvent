using Microsoft.AspNetCore.Authentication;
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

    public void SignIn(string name,string email, string role, bool rememberMe)
    {
        // (1) Claim, identity and principal
        // TODO
        List<Claim> claims = [
            new(ClaimTypes.Name,name),
            new(ClaimTypes.Email,email),
            new(ClaimTypes.Role, role)];

        // TODO
        ClaimsIdentity identity = new(claims, "Cookie");

        // TODO
        ClaimsPrincipal principal = new(identity);

        // (2) Remember me (authentication properties)
        // TODO
        AuthenticationProperties properties = new()
        {
            IsPersistent = rememberMe,
        };

        // (3) Sign in
        // TODO
        ct.HttpContext!.SignInAsync(principal, properties);
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
