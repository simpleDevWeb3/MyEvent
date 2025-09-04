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

        // Remove the same event appear
        const sanitize_data = this._data.filter(e => e.EventId !== Current_id);

        if (sanitize_data.length== 0) {
           $(this._parentEl).append(this._renderNotExist());

        }
        else {
            sanitize_data.map(e => {
                console.log(e);
                $(this._parentEl).append(this._htmlMarkup(e));
            });
        }
      
    }
    _renderNotExist() {
        return `
        <div class="card-events">
            Event Not Found
        </div>
        `
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
            // Get full query string from the current URL
            const params = new URLSearchParams(window.location.search);

            // Get the value of "id"
            const Current_id = params.get("id");

            const category = $('.category').data('tag');

            const button = e.target
            if (!button.classList.contains('home-button')) return;
            if ($('.home-button').hasClass('tag-active')) {
                $('.home-button').removeClass('tag-active');
            }
            console.log(e.target);
           
            button.classList.add("tag-active")
            const tag = e.target.dataset.tag;
            handler(tag,Current_id);
        })
    }

    addHandlerDefault(handler=0) {
        $(document).ready(() => {
            // Get full query string from the current URL
            const params = new URLSearchParams(window.location.search);

            // Get the value of "id"
            const Current_id = params.get("id");

            console.log($('.home-button').first())
            $('.home-button').eq(8).addClass('tag-active')

            handler("Participant Also Joined",Current_id);
           
        })
    }
}

export default new RecommendView();