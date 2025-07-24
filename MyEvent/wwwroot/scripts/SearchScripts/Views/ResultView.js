
class ResultView {
    _ParentEl = $('.sidebar-result')[0];

    render(data,paging) {
        console.log(data);
        const startingPage = (paging.Page - 1) * paging.ResultPerPage;

        const endPage = paging.Page * paging.ResultPerPage;
        const result = data.slice(startingPage, endPage)
     
        const events = result.map((e, i) => {

            return ` <div class="event" data-event-id = ${e.EventId} data-ajax-page="/Home/${e.Title}?id=${e.EventId}">

                        <div class="event-img-container">
                            <img src="${e.ImageUrl}">
                        </div>
                        <div class="event-detail">
                            <p class="title">${e.Title}</p>
                            <p class="organization">${e.Organizer}</p>
                            <p class="date">${e.Date}</p>
                        </div>
  

                </div>`


        });

        $(this._ParentEl)
            .children(".sidebar__events")
            .empty()
            .append(events.join(''));
            
    }


    toggle() {

        $(this._ParentEl)
            .css('transform', 'translateX(-500px)')
            .children(".sidebar-small")
            .html(` <i class="ri-arrow-right-line"></i>`)

           
          

        $('.sidebar-small')
            .removeClass('hide')
            .css('display', 'flex');
    }

    expand() {

 

        $(this._ParentEl)
            .css("transform" , "translateX(0px)")
            .children(".sidebar-small")
            .html(
                `
                     <i class= "ri-arrow-left-line arrow-left-navbar"></i >
                 `
            )
       

    }

    highlightEvent(eventId) {

        $('.event.active').removeClass('active');

        const target = $(this._ParentEl).find(`.event[data-event-id="${eventId}"]`)
        target.toggleClass('active');

    }

    AddToggleSidebar(handler) {

        $('.sidebar-small').on('click',(e)=> {
            if (!e.target.classList.contains('arrow-left-navbar')) return;
         
            this.toggle();
            handler();


        })
    }

   
   
    AddExpandSidebar(handler) { 

        $('.sidebar-small').on('click',  (e)=> {
            this.expand();
            handler();

        })
    }

    AddHover(handler) {
        $(this._ParentEl).on('mouseenter','[data-event-id]',(e) => {
            //EventDetailView.renderEventDetail(event);
            const eventId = e.target.dataset.eventId;
            console.log(eventId);
            handler(eventId);
           
        });
    }
  


  
} 

export default new ResultView();