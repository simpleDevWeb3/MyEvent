class CarouselView {

    _parentEl = $('.carousel-container')[0];
    _currentSlide = 0;

    toSlide() {
        $('.card').each((i, e) => {

            $(e).css({
                transform: `translateX(${(i - this._currentSlide) * 1400}px)`,

            });
        })
    }
    addHandleScroll() {
        $(this._parentEl).on('click', (e) => {
            if (!e.target.closest('.next-btn')) return;
            console.log($('.card').length);
            
        
            console.log(this._currentSlide);
            if (this._currentSlide < $('.card').length-1) {
                this._currentSlide++;
                this.toSlide();
            }
        })
        .on('click', (e) => {
            if (!e.target.closest('.prev-btn')) return;
            console.log('prev');
            console.log(this._currentSlide);
          
            if (this._currentSlide > 0) {
                this._currentSlide--;
                this.toSlide();
            }
          
        })
    }
}

export default new CarouselView();