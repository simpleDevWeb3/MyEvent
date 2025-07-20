class NavView {

    _parentEl = $('.nav-type')[0];


    addHandlerHover() {
        $(this._parentEl)
            .on('mouseenter', '[data-label]', function (e) {
                $('.custom-underline').removeClass('custom-underline');
                $(e.currentTarget).addClass('custom-underline');
              
            })
            .on('mouseout', '[data-label]', function (e) {
                $(e.currentTarget).removeClass('custom-underline');
            });
    }
    addHandlerClick(handler) {
        $(this._parentEl).on('click', '[data-label]', function (e) {
            const label = e.target.dataset.label;
            // Clear previous selected
            $('[data-label]').removeClass('selected custom-underline');

            // Set new selected
            $(e.currentTarget).addClass('selected custom-underline');
            handler(label);
        });
    }

}

export default new NavView();