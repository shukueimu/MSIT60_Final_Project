﻿using System;
using System.Collections.Generic;

namespace iTable_V2.Models;

public partial class Restaurant
{
    /// <summary>
    /// 餐廳的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個餐廳，並在資料表之間建立關聯。
    /// </summary>
    public int RestaurantID { get; set; }

    /// <summary>
    /// 餐廳名稱（前台 + 後台）。此欄位存儲餐廳的名稱，用於顯示和管理。
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 餐廳地址（前台 + 後台）。此欄位存儲餐廳的實體地址，用於定位和導航。
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// 聯絡電話（前台 + 後台）。此欄位存儲餐廳的聯絡電話號碼，用於客戶諮詢和緊急聯絡。
    /// </summary>
    public string? ContactPhone { get; set; }

    /// <summary>
    /// 餐廳簡介（前台 + 後台）。此欄位存儲餐廳的詳細介紹，包括特色、歷史等資訊，供客戶參考。
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 是否有停車場（1：有，0：無）（前台 + 後台）。此欄位指示餐廳是否提供停車設施，供客戶參考。
    /// </summary>
    public bool? HasParking { get; set; }

    /// <summary>
    /// 是否開放預訂（1：開放，0：關閉）（前台 + 後台）。此欄位指示餐廳是否接受預訂，供客戶決定是否進行預約。
    /// </summary>
    public bool IsReservationOpen { get; set; }

    /// <summary>
    /// 餐廳的平均評分（前台 + 後台）。此欄位存儲餐廳的平均客戶評分，供客戶參考和比較。
    /// </summary>
    public double? AverageRating { get; set; }

    /// <summary>
    /// 餐廳資料建立時間（前台 + 後台）。此欄位記錄餐廳資料的建立時間，用於審計和管理。
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 餐廳資料更新時間（前台 + 後台）。此欄位記錄餐廳資料的最後更新時間，用於追蹤資料變更歷史。
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    public TimeOnly BusinessHoursStart { get; set; }

    public TimeOnly BusinessHoursEnd { get; set; }

    public TimeOnly? LastCheckInTime { get; set; }

    public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

    public virtual ICollection<ReservationControlSetting> ReservationControlSettings { get; set; } = new List<ReservationControlSetting>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<RestaurantAvailability> RestaurantAvailabilities { get; set; } = new List<RestaurantAvailability>();

    public virtual ICollection<RestaurantBusinessHour> RestaurantBusinessHours { get; set; } = new List<RestaurantBusinessHour>();

    public virtual ICollection<RestaurantUser> RestaurantUsers { get; set; } = new List<RestaurantUser>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
