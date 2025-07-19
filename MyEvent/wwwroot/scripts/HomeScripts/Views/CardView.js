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
           

            <div class="home-event" data-event-id=${e.EventId}>
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
   
            console.log("load")
            handler();
        })
    }
    addHandleClick() {
        $(document).on('click', '[data-event-id]', (e) => {
            const eventId = e.currentTarget.dataset.eventId;
            location.href = `/Home/EventDetail?id=${eventId}`;
        });
    }
}

export default new CardView();