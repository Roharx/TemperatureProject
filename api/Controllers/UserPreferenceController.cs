using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace api.Controllers;


[Authorize]
[Route("api/[controller]")]
public class UserPreferenceController
{
    //maybe later, not important for now
}