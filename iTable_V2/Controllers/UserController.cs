﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using iTable_V2.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace iTable_V2.Controllers
{
    public class UserController : Controller
    {
        private readonly ITableDbContext _context;

        public UserController(ITableDbContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }


        //// POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,PasswordHash,Name,ContactPhone,ContactEmail")] User user)
        {
            if (ModelState.IsValid)
            {
                // 設定 CreatedAt 和 UpdatedAt
                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;

                // 儲存使用者資料（密碼會直接儲存）
                _context.Add(user);
                await _context.SaveChangesAsync();

                // 註冊成功後可以重定向到註冊成功頁面或其他頁面
                return RedirectToAction(nameof(RegisterSuccess)); // 或者可以重定向到顯示所有使用者的頁面
            }

            // 若資料無效，重新顯示註冊頁面
            return View(user);
        }

        public IActionResult RegisterSuccess()
        {
            return View();
        }

        // POST: User/Edit/5--------------------------------------------
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,PasswordHash,Name,ContactPhone,ContactEmail,CreatedAt,UpdatedAt")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // POST: User/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("UserID,UserName,Name,ContactPhone,ContactEmail,CreatedAt,UpdatedAt")] User user)
        //{
        //    if (id != user.UserID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // 更新 UpdatedAt 時間
        //            user.UpdatedAt = DateTime.Now;

        //            // 更新使用者資料
        //            _context.Update(user);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(user.UserID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        // 更新成功後返回到會員資料頁面或其他頁面
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        // GET: User/Delete/5---------------------------------------------
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }

        //登入畫面-----------------------------
        // GET: User/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserName,PasswordHash")] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 檢查帳號和密碼是否匹配
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == model.UserName && u.PasswordHash == model.PasswordHash);

                if (user != null)
                {
                    // 登入成功，將 UserID 儲存到 Session
                    HttpContext.Session.SetInt32("UserID", user.UserID);

                    // 重定向到訂位頁面（暫時的URL）
                    return Redirect("http://localhost:5169/Booking/BookingPage/1");
                }

                // 登入失敗，顯示錯誤訊息
                ModelState.AddModelError("", "帳號或密碼錯誤");
            }

            // 如果有問題，重新顯示登入頁面
            return View(model);
        }

        //登出-----------------------------------------
        public IActionResult Logout()
        {
            // 清除所有 Session 資料
            HttpContext.Session.Clear();

            // 設定成功訊息
            //TempData["Message"] = "您已成功登出！";

            // 跳轉到登入頁面
            return RedirectToAction("Login", "User");
        }

        //修改會員資料-----------------------------------
       // GET: User/EditProfile
       //[HttpGet]
        public IActionResult EditProfile()
        {
            // 顯示使用者資料
            var userID = HttpContext.Session.GetInt32("UserID");

            if (userID == null)
            {
                return RedirectToAction("Login");
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.UserID == userID);
            if (existingUser == null)
            {
                return NotFound();
            }

            return View(existingUser);
        }

        // POST: User/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile([Bind("UserID, UserName, Name, ContactPhone, ContactEmail, PasswordHash, CreatedAt, UpdatedAt")] User user)
        {
            var userID = user.UserID;

            if (userID == 0)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userID);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // 更新資料
                    existingUser.UserName = user.UserName;
                    existingUser.Name = user.Name;
                    existingUser.ContactPhone = user.ContactPhone;
                    existingUser.ContactEmail = user.ContactEmail;
                    existingUser.PasswordHash = user.PasswordHash; // 更新密碼哈希值
                    existingUser.CreatedAt = user.CreatedAt;       // 保留原始的 CreatedAt 值
                    existingUser.UpdatedAt = DateTime.Now;         // 更新 UpdatedAt 為當前時間

                    await _context.SaveChangesAsync();

                    TempData["Message"] = "會員資料更新成功！";
                    return RedirectToAction("EditProfile");
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "更新資料時發生錯誤，請稍後再試。");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "發生未知錯誤: " + ex.Message);
                }
            }

            return View(user);
        }

        //-----------------------------------------------------------------------





    }
}
