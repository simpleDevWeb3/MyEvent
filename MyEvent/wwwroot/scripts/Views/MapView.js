
import EventDetailView from "./EventDetailView.js";
import SidebarView from "./SidebarView.js";
class MapView {
    _map;
    _parentEl = $('#map')[0];
    _zoomLvl = 13;
    _zoomTo = 15;
   


    render(data) {
        this._data = data;
        this._LoadMap();
    }

    MoveToCoords(lat,lng) {
        this._map.setView([lat, lng], this._zoomTo);
      
    }

    _LoadMap() {
        if (this._map !== undefined) {
            this._map.remove();
        }
        console.log(this._data);
        const {lat,lng} = this._data.coords;

        this._map = L.map(this._parentEl).setView([lat,lng], this._zoomLvl);

        L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(this._map);

        this._data.Events.map(event=> {
            this._RenderMarker(event);
        })

    }

    _RenderMarker(event) {
        console.log('🧭 _RenderMarker was called:', event);
        console.trace('Trace for marker source');
       L.marker([event.Latitude, event.Longitude]
        ).addTo(this._map)
            .bindPopup(
                L.popup({
                    maxWidth: 250,
                    minWidth: 100,
                    autoClose: true,
                    closeOnClick: false,

                }))
            .setIcon(
                L.icon({
                    iconUrl: event.ImageUrl,
                    iconSize: [40, 40],
                    iconAnchor: [20, 40],
                    popupAnchor: [0, -40],
                    className: 'circular-icon'
                })
            )
            .setPopupContent(`${event.Title}`)
            .on('click', () => {
                EventDetailView.renderEventDetail(event);
                this.MoveToCoords(event.Latitude, event.Longitude);
                SidebarView.highlightEvent(event.EventId);
            })

        
    }

 

    _ResizeObserver() {

        const resizeObserver = new ResizeObserver(() => {
            this._map.invalidateSize();
        });

        const mapDiv = $('.map-container')[0];
        resizeObserver.observe(mapDiv);

    }

   
}

export default new MapView();