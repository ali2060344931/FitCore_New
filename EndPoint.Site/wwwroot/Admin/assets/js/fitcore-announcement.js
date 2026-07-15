window.addEventListener("DOMContentLoaded", function () {

    if (window.fcAnnouncementLoaded)
        return;

    window.fcAnnouncementLoaded = true;

    const AutoCloseSeconds = 120;

    const popup = document.getElementById("fcAnnouncement");

    if (!popup)
        return;

    const announcementId = popup.dataset.id;

    const progress =
        popup.querySelector(".fc-progress-bar");

    const closeBtn =
        popup.querySelector(".fc-close");

    const actionBtn =
        popup.querySelector(".fc-btn");

    //-------------------------------------------------
    // جلوگیری از نمایش مجدد در همین Session
    //-------------------------------------------------

    const storageKey =
        "fc-announcement-" + announcementId;

    if (sessionStorage.getItem(storageKey)) {

        popup.remove();

        return;
    }

    sessionStorage.setItem(storageKey, "1");

    //-------------------------------------------------
    // نمایش Popup
    //-------------------------------------------------

    popup.classList.add("show");

    //-------------------------------------------------
    // ثبت مشاهده فقط یکبار
    //-------------------------------------------------

    let viewRegistered = false;

    function registerView() {

        if (viewRegistered)
            return;

        viewRegistered = true;

        fetch("/Admin/Announcement/RegisterView/" + announcementId, {

            method: "POST",

            headers: {

                "X-Requested-With": "XMLHttpRequest"

            }

        });

    }

    registerView();

    //-------------------------------------------------
    // بستن Popup
    //-------------------------------------------------

    function closePopup(registerDismiss = false) {

        popup.classList.add("hide");

        if (registerDismiss) {

            fetch("/Admin/Announcement/Dismiss/" + announcementId, {

                method: "POST",

                headers: {

                    "X-Requested-With": "XMLHttpRequest"

                }

            });

        }

        setTimeout(function () {

            popup.remove();

        }, 300);

    }

    //-------------------------------------------------
    // دکمه بستن
    //-------------------------------------------------

    if (closeBtn) {

        closeBtn.addEventListener("click", function () {

            closePopup(true);

        });

    }

    //-------------------------------------------------
    // دکمه اقدام
    //-------------------------------------------------

    if (actionBtn) {

        actionBtn.addEventListener("click", function () {

            closePopup(false);

        });

    }

    //-------------------------------------------------
    // کلیک روی Overlay
    //-------------------------------------------------

    popup.querySelector(".fc-announcement-overlay")
        ?.addEventListener("click", function () {

            closePopup(true);

        });

    //-------------------------------------------------
    // ProgressBar
    //-------------------------------------------------

    if (progress) {

        progress.style.transition =
            "width " + AutoCloseSeconds + "s linear";

        setTimeout(function () {

            progress.style.width = "0%";

        }, 50);

    }

    //-------------------------------------------------
    // Auto Close
    //-------------------------------------------------

    setTimeout(function () {

        closePopup(false);

    }, AutoCloseSeconds * 1000);

});