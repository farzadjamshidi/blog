using System.Security.Claims;
using Blog.API.Services;
using Blog.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Blog.API.Controllers;

[ApiController]
[Route (template: "[controller]")]
public class AccountsController : ControllerBase
{
    private readonly AppDbContext _ctx;
    private readonly UserManager<IdentityUser> _userManager; 
    private readonly RoleManager<IdentityRole> _roleManager; 
    private readonly SignInManager<IdentityUser> _signInManager; 
    private readonly IdentityService _identityService;

    public AccountsController(
        AppDbContext ctx,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IdentityService identityService,
        SignInManager<IdentityUser> signInManager
    )
    {
        _ctx = ctx;
        _userManager = userManager;
        _roleManager = roleManager;
        _identityService = identityService;
        _signInManager = signInManager;
    }


    [HttpPost]
    [Route(template: "register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
    {
        // Create new IdentityUser. This will persist the user to the database.
        var identity = new IdentityUser {Email = registerUser.Email, UserName = registerUser.Email};
        var createdIdentity = await _userManager.CreateAsync(identity, registerUser.Password);
        
        // We want to add first name and last name as claims to the user. These claims also need to be persisted.
        var newClaims = new List<Claim>
        {
            new("FirstName", registerUser.FirstName),
            new("LastName", registerUser.LastName)
        };
        await _userManager.AddClaimsAsync(identity, newClaims);
        
        // We want to add the user to a role. If the role does not exist, we want to create it.
        if (registerUser.Role == Role.Administrator)
        {
            var role = await _roleManager.FindByNameAsync("Administrator");
            if (role == null)
            {
                role = new IdentityRole("Administrator");
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(identity, role: "Administrator");

            // add the newly added role to the claims
            newClaims.Add(item: new Claim(ClaimTypes.Role, "Administrator"));
        }
        else
        {
            var role = await _roleManager.FindByNameAsync("User");
            if (role == null)
            {
                role = new IdentityRole("User");
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(identity, role: "User");

            // add the newly added role to the claims
            newClaims.Add(item: new Claim(ClaimTypes.Role, "User"));
        }
        
        // Create a ClaimsIdentity to be used when generating a JWT.
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, value: identity.Email ?? throw new InvalidOperationException()),
            new(JwtRegisteredClaimNames.Email, value: identity.Email ?? throw new InvalidOperationException())
        });
        
        //also add the claims for first name and last name claimsIdentity.AddClaims(newClaims);
        var token = _identityService.CreateSecurityToken(claimsIdentity);
        var response = new AuthenticationResult(_identityService.WriteToken(token));
        
        return Ok(response);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginUser login)
    {
        var user = await _userManager.FindByEmailAsync(login.Email);
        if (user is null) return BadRequest();

        var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
        if (!result.Succeeded) return BadRequest("Couldn't sign in.");

        var claims = await _userManager.GetClaimsAsync(user);

        var roles = await _userManager.GetRolesAsync(user);

        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Email ?? throw new InvalidOperationException()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? throw new InvalidOperationException())
        });
        
        claimsIdentity.AddClaims(claims);
        
        foreach(var role in roles)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        var token = _identityService.CreateSecurityToken(claimsIdentity);

        var response = new AuthenticationResult(_identityService.WriteToken(token));

        return Ok(response);
    }
    
    public enum Role
    {
        Administrator,
        User
    }
    public record RegisterUser(string Email, string Password, string FirstName, string LastName, Role Role);
    public record LoginUser(string Email, string Password);
    public record AuthenticationResult(string Token);