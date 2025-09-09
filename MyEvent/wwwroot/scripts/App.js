

const Router = function () {
    // Intercept internal link clicks
    $(document).on('click', '[data-ajax-page]', async function (e) {
        e.preventDefault();
        const url = $(this).data('ajax-page');
        // Get current path + query string
        const current = location.pathname + location.search;

        // If already on the page, skip
        if (current === url) return;

        location.href = url;
    });

};

// Initiate GET request (AJAX-supported)
$(document).on('click', '[data-get]', e => {
    e.preventDefault();
    const url = e.target.dataset.get;
    location = url || location;
});

// Initiate POST request (AJAX-supported)
$(document).on('click', '[data-post]', e => {
    e.preventDefault();
    const url = e.target.dataset.post;
    const f = $('<form>').appendTo(document.body)[0];
    f.method = 'post';
    f.action = url || location;
    f.submit();
});

// Trim input
$('[data-trim]').on('change', e => {
    e.target.value = e.target.value.trim();
});

// Auto uppercase
$('[data-upper]').on('input', e => {
    const a = e.target.selectionStart;
    const b = e.target.selectionEnd;
    e.target.value = e.target.value.toUpperCase();
    e.target.setSelectionRange(a, b);
});

// RESET form
$('[type=reset]').on('click', e => {
    e.preventDefault();
    location = location;
});

// Check all checkboxes
$('[data-check]').on('click', e => {
    e.preventDefault();
    const name = e.target.dataset.check;
    $(`[name=${name}]`).prop('checked', true);
});

// Uncheck all checkboxes
$('[data-uncheck]').on('click', e => {
    e.preventDefault();
    const name = e.target.dataset.uncheck;
    $(`[name=${name}]`).prop('checked', false);
});

// Row checkable (AJAX-supported)
$(document).on('click', '[data-checkable]', e => {
    if ($(e.target).is(':input,a')) return;

    $(e.currentTarget)
        .find(':checkbox')
        .prop('checked', (i, v) => !v);
});

// Photo preview
$('.upload input').on('change', e => {
    const f = e.target.files[0];
    const img = $(e.target).siblings('img')[0];

    img.dataset.src ??= img.src;

    if (f && f.type.startsWith('image/')) {
        img.onload = e => URL.revokeObjectURL(img.src);
        img.src = URL.createObjectURL(f);
    }
    else {
        img.src = img.dataset.src;
        e.target.value = '';
    }

    // Trigger input validation
    $(e.target).valid();
});


Router();


let timeout;



/////////////////////////////////////////////////Cheng's Code
let auto_submit = null;
$('#name, #create_event_location_search').on('input', e => {
    clearTimeout(auto_submit);
    auto_submit = setTimeout(() => $(e.target.form).submit(), 200);
});

// Show overlay when readonly input is focused (clicked)
$(".create_event_location_input").on("focus click", e => {
    $("#overlay").fadeIn();
    $("body").css("overflow", "hidden");
});

// Close overlay
$(document).on("click", ".create_event_location_result", e => {
    $("#overlay").fadeOut();
    $("body").css("overflow", "auto");
});

// Close overlay when clicking outside form
$("#overlay").on("click", function (e) {
    if (e.target.id === "overlay") {
        $(this).fadeOut();
        $("body").css("overflow", "auto");
    }
});

$(document).on('click', '.create_event_location_result', function () {
    let formatted = $(this).text().trim();
    $('.create_event_location_input').val(formatted);
});

$("input[type='time']").on("blur", e => {

    let a = $(e.target).data("attribute");
    $(".validation_msg").css("display", "none");
    $("." + a).css("display", "inline");

});

$(".create_event-backBtn").on("click", function () {
    //window.history.back();
    var prevUrl = document.referrer; // previous page URL

    if (prevUrl) {
        window.location.href = prevUrl; // go to it
    } else {
        // fallback if no referrer available
        window.location.href = "/";
    }
});



///////////////////////////////////////////////////Ken Code/////
$('.profile-picture').on('click', () => {
    $('.modal-user').toggleClass('hide');
})

