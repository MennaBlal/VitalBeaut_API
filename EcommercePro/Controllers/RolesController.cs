using EcommercePro.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EcommercePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private  RoleManager<IdentityRole> roleManager;
       public  RolesController(RoleManager<IdentityRole> _roleManager) { 
             this.roleManager = _roleManager;

        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult<List<Role>> GetAll()
        {
            List<Role> roles = this.roleManager.Roles.Select(role => new Role()
            {
                Id = role.Id,
                Name = role.Name

            }).ToList();

            return roles;
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Add(Role newRole)
        {
            if (newRole.Name != null)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = newRole.Name
                };
              IdentityResult result =  await this.roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Ok();

                }
                else
                {
                    return BadRequest(result.Errors);
                }
 
            }
            
            return BadRequest("Enter the Role Name");

            
         }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id, Role role)
        {
            if (id != null)
            {
             IdentityRole roledb =   await this.roleManager.FindByIdAsync(id);
                if (roledb != null)
                {
                    roledb.Name = role.Name;
                    var result = await this.roleManager.UpdateAsync(roledb);
                    if (result.Succeeded)
                    {
                        // Role updated successfully
                        return Ok();
                    }
 
                }



            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async  Task<IActionResult> Delete(string id)
        {
            if(id != null)
            {
                IdentityRole roledb = await this.roleManager.FindByIdAsync(id);

                if (roledb != null)
                {
                  IdentityResult result =  await this.roleManager.DeleteAsync(roledb);
                    if (result.Succeeded)
                    {
                        return Ok();

                    }


                }
            }
            return BadRequest();
        }
    }
}
