class RecommendView {
    _parentEl = $('.Event-List-Container')[0];
    _data;

    render(data) {
        this._data = data;
        this._renderMarkup();
    }

    _renderMarkup() {
        console.log(this._data)
        $(this._parentEl).empty();

        // Get full query string from the current URL
        const params = new URLSearchParams(window.location.search);

        // Get the value of "id"
        const Current_id = params.get("id");

        this._data.map(e => {
            console.log(e);
            //display relevent event without current event
            if (e.EventId == Current_id) return;
            $(this._parentEl).append(this._htmlMarkup(e));
        });
    }

    _htmlMarkup(e) {
        return `
        <div class="card-events" data-ajax-page="/Home/${e.Title}?id=${e.EventId}">
            <div class="image-container" style="height:100px;">
                <img class="event-detail__img" style="border-radius:8px;" src="${e.ImageUrl}" />
            </div>
            <div>
                <div class="home-event-title">${e.Title}</div>

                <div>
                     ${e.City}, ${e.Street}
                </div>
            </div>
        </div>
        `
    }

    addHandlerDisplay(handler = 0) {
        $('.nav-recommendation').on('click', (e) => {
            const button = e.target
            if (!button.classList.contains('home-button')) return;
            if ($('.home-button').hasClass('tag-active')) {
                $('.home-button').removeClass('tag-active');
            }
            console.log(e.target);
           
            button.classList.add("tag-active")
            const tag = e.target.dataset.tag;
            handler(tag);
        })
    }

    addHandlerDefault(handler=0) {
        $(document).ready(() => {
            const category = $('.category').data('tag');

            handler(category);
           
        })
    }
}

export default new RecommendView();