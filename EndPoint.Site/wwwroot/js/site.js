// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ajaxError(function (event, jqxhr) {
    if (jqxhr.status === 401) {
        Swal.fire({
            icon: 'warning',
            title: 'نشست شما منقضی شده است',
            text: 'لطفاً برای ادامه کار دوباره وارد حساب کاربری خود شوید.',
            confirmButtonText: 'ورود مجدد',
            confirmButtonColor: '#3085d6'
        }).then((result) => {
            window.location.href = '/Admin/Auth/Login';
        });
    }
});

