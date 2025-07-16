

const Router = function () {
    // Intercept internal link clicks
    $(document).on('click', '[data-ajax-page]', async function (e) {
        e.preventDefault();
        const url = $(this).data('ajax-page');
        location.href = url;
        
    });

};

Router();