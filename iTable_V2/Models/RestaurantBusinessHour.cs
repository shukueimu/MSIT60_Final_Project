using System;
using System.Collections.Generic;

namespace iTable_V2.Models;

public partial class RestaurantBusinessHour
{
    /// <summary>
    /// 營業時間的唯一識別碼，主鍵（PK），自動遞增。此欄位用於唯一標識每個營業時間紀錄，並在資料表之間建立關聯。
    /// </summary>
    public int BusinessHourId { get; set; }

    /// <summary>
    /// 餐廳ID，外鍵（FK）連結到 Restaurants 表（前台 + 後台）。此欄位指定該營業時間所屬的餐廳。
    /// </summary>
    public int? RestaurantId { get; set; }

    /// <summary>
    /// 星期幾（1-7，1 代表星期一）（前台 + 後台）。此欄位指定營業時間適用的星期幾，方便客戶查詢。
    /// </summary>
    public int? WeekDay { get; set; }

    /// <summary>
    /// 營業開始時間（前台 + 後台）。此欄位存儲餐廳每天的營業開始時間，供客戶查詢和預約使用。
    /// </summary>
    public TimeOnly? OpenTime { get; set; }

    /// <summary>
    /// 營業結束時間（前台 + 後台）。此欄位存儲餐廳每天的營業結束時間，供客戶查詢和預約使用。
    /// </summary>
    public TimeOnly? CloseTime { get; set; }

    /// <summary>
    /// 資料建立時間（前台 + 後台）。此欄位記錄營業時間紀錄的建立時間，用於審計和管理。
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 資料更新時間（前台 + 後台）。此欄位記錄營業時間紀錄的最後更新時間，用於追蹤資料變更歷史。
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    public virtual Restaurant? Restaurant { get; set; }
}
