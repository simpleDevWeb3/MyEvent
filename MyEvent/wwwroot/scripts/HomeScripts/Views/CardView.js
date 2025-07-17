class CardView {

    _parentEl = $('.event--all')[0];
   

    render(data) {
        this._data = data;
        this._RenderCardEvents();
    }

    _RenderCardEvents() {
        $(this._parentEl).empty();
    
        this._data.map(e =>
            $(this._parentEl).append(this._htmlMarkup(e))
        );
    }

    _htmlMarkup(e) {


        return `
           

            <div class="home-event">
                <div class="card-image">
                    <img class="home-event-img" src="${e.ImageUrl}">
                </div>
                
                <div class="home-event-title">${e.Title}</div>
                <div>
                     <i class="ri-calendar-line"></i>
                     ${e.Date}
                </div>
                <div>
                    ${e.City}, ${e.Street}
                </div>
            </div>

      `
    }
    





    addHandlerDisplay(handler) {

        $(window).on('load', (e) => {
            e.preventDefault();
            console.log("load")
            handler();
        })
    }
}

export default new CardView();