

class NavBarView {

    _parentEl = $('.navbar')[0];

    AddHandlerHome(handler) {
        $(this._parentEl).on('click', function (e) {
            const target = e.target
            if (!target.classList.contains('home')) return;
            console.log(target);
            $('.locate__btn').toggleClass('hide');
            $(target).toggleClass('hide');

            handler();
        })
    }

    AddHandlerOpenMap(handler) {
        $(this._parentEl).on('click', function (e) {
            const target = e.target.parentElement
            if (!target.classList.contains('locate__btn')) return;
            console.log(target);
            handler();
        })
    }

}

export default new NavBarView();