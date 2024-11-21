//document.addEventListener('DOMContentLoaded', function () {
//    const loginBtn = document.getElementById('loginBtn');
//    const loggedOut = document.getElementById('loggedOut');
//    const loggedIn = document.getElementById('loggedIn');
//    const logoutItem = document.querySelector('.logout-item');

//    // 模擬檢查登入狀態（這部分應該由後端返回狀態，可用 AJAX 或設定值）
//    let isLoggedIn = false; // 假設初始為未登入

//    // 初始狀態切換
//    function updateUI() {
//        if (isLoggedIn) {
//            loggedOut.style.display = 'none';
//            loggedIn.style.display = 'block';
//        } else {
//            loggedOut.style.display = 'block';
//            loggedIn.style.display = 'none';
//        }
//    }

//    updateUI(); // 頁面加載時執行

//    // 點擊登入按鈕
//    loginBtn.addEventListener('click', function () {
//        if (!isLoggedIn) {
//            // 模擬跳轉至登入頁面
//            window.location.href = "/User/Login";
//        }
//    });

//    // 登出按鈕功能
//    logoutItem.addEventListener('click', function () {
//        isLoggedIn = false; // 模擬登出
//        updateUI(); // 更新 UI
//    });

//    // 模擬登入成功後（你應該在後端返回真實狀態）
//    window.simulateLogin = function () {
//        isLoggedIn = true; // 模擬登入
//        updateUI(); // 更新 UI
//    };
//});
