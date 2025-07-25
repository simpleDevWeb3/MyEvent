
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

    _RenderError(err) {
        err = err.message.split(':');
        $('.title-category').empty();
        $(this._parentEl).empty().append(`

             <h1>${err}</h1>
        `)
    }

    _htmlMarkup(e) {


        return `
           

            <div class="home-event"  data-ajax-page="/Home/${e.Title}?id=${e.EventId}">
                <div class="card-image" style="position:relative">
                    <img class="home-event-img" src="${e.ImageUrl}">
                    <div class="home-event-date">
                    
                        <p  style="margin:0; font-size:14px;">${dayjs(e.Date).format("MMM")}</p>
                        <h3 style="margin:0"><b>${dayjs(e.Date).format("DD")}</b></h3>
                         <p style="margin:0; opacity: 0.6;">${dayjs(e.Date).format("ddd")}</p>
                    </div>
                </div>
                
                <div class="home-event-title">${e.Title}</div>
           
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
 
}

export default new CardView();