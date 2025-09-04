class AttandeView {
    _parentEl = $('.Attande-Container')[0];
    _data;


    render(data) {
        this._data = data;
        this._renderMarkup();
    }

    //Render list of attande
    _renderMarkup() {
        console.log(this._data,"from")
        this._data.map(p => {

            $(this._parentEl).append(this._markup(p));
        });
    }

    //Render attande 
    _markup(p) {
       
        return `
       
               <div class="Attande-card" style="  margin-top:10px; width:100px; display:flex; flex-direction:column; justify-content:center; align-items:center; gap:10px;">
                                <div class="profile-container" style="height:80px; width:80px">
                                    <img class="event-detail__img" style="border-radius:50%;" src="${p.PhotoURL}" />
                                </div>
                                <div style="text-align:center;">
                                    <div>${p.Name}</div>
                                    <div style="opacity:0.8;">Participant</div>
                                </div>


                            </div>

                      

               </div>
        
        `
    }

    addHandler(handler=0) {
        $(document).ready(() => {
            // Get full query string from the current URL
            const params = new URLSearchParams(window.location.search);

            // Get the value of "id"
            const Current_id = params.get("id");

            handler(Current_id);
        })
    }
}

export default new AttandeView();