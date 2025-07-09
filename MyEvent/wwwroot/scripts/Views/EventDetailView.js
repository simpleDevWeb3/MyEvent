


class EventDetailView  {
    _ParentEl = $('.sidebar-details')[0];
    

    render(data) {
        this._data = data;
        this.renderEventDetail(data);
    }

    renderEventDetail(event) {

        const detail = `

        <header class="arrow-container">
               
                <i class="ri-close-circle-fill close-circle-eventDetail"></i>
                
        </header>
         <div class="event-detail-container">
                <div class="event-detail-image-container">
                 <img class="event-detail-image" src="${event.ImageUrl}">
                </div>
                <div class="event-detail" >
                        <p class="title">${event.Title}</p>
                        <p class="organization">${event.Organizer}</p>
                        <p class="date">${event.Date}</p>
                        <p class="description">${event.Description}</p>
                        <p class="time">${event.StartTime} - ${event.EndTime} </p>
                        <div class="address">
                            <p>${event.Street},${event.City}</p>
                        </div>
                  </div> 
          </div>
        `
        this.toggleEventDetail(detail);
    }


    toggleEventDetail(detail = '') {
        if (detail) {
            $(this._ParentEl)
                .css("display", "flex")
                .children(".sidebar__event-detail")
                .empty()
                .append(detail)
                .css("display", "flex");

        } else {
            $(this._ParentEl)
                .css("display", "none");
        }
    }


    AddToggleDetail() {
        $(this._ParentEl).on('click', (e) => {
            if (!e.target.classList.contains('close-circle-eventDetail')) return;

            this.toggleEventDetail();



        })
    }

 


}

export default new EventDetailView;