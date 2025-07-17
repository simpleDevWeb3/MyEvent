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
    activeTracker() {
        const tracker = $(`.tracker[data-card-id="${this._currentSlide}"]`)[0];
     

        $('.tracker').removeClass('activeTracker')[0];


         $(tracker).addClass('activeTracker');
        

    }
    autoScroll() {
        setInterval(() => {
            const totalSlides = $('.card').length;

            this._currentSlide = (this._currentSlide + 1) % totalSlides;
            this.toSlide();
            this.activeTracker();
        }, 5000);
    }

       

    genereateTracker(){
        $('.card').each((i) => {
            
            $('.curosoulle-tracker').append(`<div class="tracker" data-card-id=${i}></div>`);
            if (i === 0) $('.tracker').addClass('activeTracker')
        })
    }

    addHandleScroll() {
        this.genereateTracker();
        $(this._parentEl).on('click', (e) => {
            if (!e.target.closest('.next-btn')) return;
            console.log($('.card').length);
            
        
            console.log(this._currentSlide);
            if (this._currentSlide < $('.card').length-1) {
                this._currentSlide++;
                this.toSlide();
                this.activeTracker();
            }
        })
        .on('click', (e) => {
            if (!e.target.closest('.prev-btn')) return;
            console.log('prev');
            console.log(this._currentSlide);
          
            if (this._currentSlide > 0) {
                this._currentSlide--;
                this.toSlide();
                this.activeTracker();
            }
          
        })

        this.autoScroll();
    }
}

export default new CarouselView();