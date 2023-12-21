using Limania.DataAccess.Data;
using Limania.DataAccess.Repository.IRepository;
using Limania.Models;
using Limania.Models.ViewModels;
using Limania.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LimaniaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class UserController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
			_unitOfWork = unitOfWork;
            _userManager = userManager;
			_roleManager= roleManager;

		}

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> RoleManager(string userId)
        {
            RoleManagmentVM RoleVM = new RoleManagmentVM()
            {
                ApplicationUser = await _unitOfWork.ApplicationUser.Get(u => u.Id == userId, includeProperties:"Company"),
                RoleList = _roleManager.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _unitOfWork.Company.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
			RoleVM.ApplicationUser.Role = (await _userManager.GetRolesAsync(await _unitOfWork.ApplicationUser.Get(x => x.Id == userId))).FirstOrDefault();
			return View(RoleVM);
        }
        [HttpPost]
		public async Task<IActionResult> RoleManager(RoleManagmentVM roleManagmentVM)
		{
            string oldRole = (await _userManager.GetRolesAsync(await _unitOfWork.ApplicationUser.Get(x => x.Id == roleManagmentVM.ApplicationUser.Id))).FirstOrDefault();
			ApplicationUser applicationUser = await _unitOfWork.ApplicationUser.Get(u => u.Id == roleManagmentVM.ApplicationUser.Id);


			if (!(roleManagmentVM.ApplicationUser.Role == oldRole))
            {
                if(roleManagmentVM.ApplicationUser.Role == SD.Role_User_Comp)
                {
                    applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                }
                if(oldRole == SD.Role_User_Comp)
                {
                    applicationUser.CompanyId = null;
                }
                _unitOfWork.ApplicationUser.Update(applicationUser);
				await _unitOfWork.Save();

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }
            else
            {
                if(oldRole == SD.Role_User_Comp && applicationUser.CompanyId != roleManagmentVM.ApplicationUser.CompanyId)
                {
                    applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                    _unitOfWork.ApplicationUser.Update(applicationUser);
                    await _unitOfWork.Save();
                }
            }

			return RedirectToAction("Index");
		}

		#region API CALLS
		public async Task<IActionResult> GetAll()
        {
            List<ApplicationUser> objUserList = _unitOfWork.ApplicationUser.GetAll(includeProperties:"Company").ToList();

            foreach (var user in objUserList)
            {
                user.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                if (user.Company == null)
                {
                    user.Company = new() { Name = "" };
                }
            }

            return Json(new { data = objUserList });
        }

        [HttpPost]
        public async Task<IActionResult> LockUnlock([FromBody] string id)
        {
            var onjFromDb = await _unitOfWork.ApplicationUser.Get(u => u.Id == id);
            if (onjFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });

            }
            if (onjFromDb.LockoutEnd != null && onjFromDb.LockoutEnd > DateTime.Now)
            {
                onjFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                onjFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _unitOfWork.ApplicationUser.Update(onjFromDb);
            await _unitOfWork.Save();
            return Json(new { success = true, message = "Operation successful" });


        }
        #endregion
    }
}
