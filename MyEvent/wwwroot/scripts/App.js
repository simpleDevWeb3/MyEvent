

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

Router();