using EcommercePro.DTO;
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
        public async Task<IActionResult> Add(string RoleName)
        {
            if (RoleName != null)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = RoleName
                };
              IdentityResult result =  await this.roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Ok();

                }
 
            }
            
            return BadRequest("Enter the Role Name");

            
         }
        [HttpPut]
        public async Task<IActionResult> Edit(string id , string RoleName)
        {
            if (id != null)
            {
             IdentityRole roledb =   await this.roleManager.FindByIdAsync(id);
                if (roledb != null)
                {
                    roledb.Name = RoleName;
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
        [HttpDelete]
        public  async  Task<IActionResult> Delete(string id)
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
