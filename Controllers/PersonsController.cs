using LoginRegister.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LoginRegister.Controllers
{
    public class PersonsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PersonsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        
        public async Task<ActionResult> Index(string email)
        {
            var users = _userManager.Users.Where(user => user.Email != email);
            var userList = new List<UserModelView>();

            foreach (var user in await users.ToListAsync())
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserModelView
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Role = roles.FirstOrDefault()
                });
            }
            return View(userList);
        }

        
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserModelView
            {
                Id = user.Id,
                Email = user.Email!,
                Role = roles.FirstOrDefault()!
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserModelView model, string selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!string.IsNullOrEmpty(selectedRole))
                    {
                        await _userManager.AddToRoleAsync(user, selectedRole);
                    }

                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser model, string selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PasswordHash = model.PasswordHash,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, model.PasswordHash!);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(selectedRole))
                    {
                        await _userManager.AddToRoleAsync(user, selectedRole);
                    }

                    return RedirectToAction("Index", "Persons");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Index");
        }
    }
}
