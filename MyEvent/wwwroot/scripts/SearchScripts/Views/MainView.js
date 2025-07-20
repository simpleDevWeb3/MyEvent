
import EventDetailView from "./EventDetailView.js";
import SidebarView from "./SidebarView.js";
class MainView {

    _parentEl = $('#map')[0];
    _zoomLvl = 13;
    _zoomTo = 15;



    render(data) {
        this._data = data;
        this._LoadEvents();
    }

    MoveToCoords(lat, lng) {
        this._map.setView([lat, lng], this._zoomTo);

    }

    _LoadEvents() {
        console.log(this._data);
        

    }




    addHandlerLocate(handler) {

        $(window).on('load', (e) => {
            e.preventDefault();
            console.log("load")
            handler();
        })
    }
}

export default new MainView();