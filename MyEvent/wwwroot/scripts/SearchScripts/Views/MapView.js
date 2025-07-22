
import SidebarView from "./SidebarView.js";
class MapView {
    _map;
    _parentEl = $('#map')[0];
    _zoomLvl = 13;
    _zoomTo = 15;
    _markers = [];


    render(data) {
        this._data = data;
        this._LoadMap();
    }

    MoveToCoords(lat,lng) {
        this._map.setView([lat, lng], this._zoomTo);
      
    }

    _LoadMap() {
       
        const lat = this._data.Search[0].Latitude;
        const lng = this._data.Search[0].Longitude;


        this._map = L.map(this._parentEl).setView([lat,lng], this._zoomLvl);
        L.tileLayer('https://{s}.tile.openstreetmap.fr/hot/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors',

        }).addTo(this._map);

        this._data.Search.map(event=> {
            this._RenderMarker(event);
            
        })

    }
    _UpdateMarkers(data) {
        // Clear previous markers from map
        this._markers.forEach(marker => this._map.removeLayer(marker));
        this._markers = [];
        console.log(data)
        // Add new markers
        data.forEach(event => {
            this._RenderMarker(event);
        });

        //  center the map to the first result
        if (this._data.Search.length > 0) {
            const first = this._data.Search[0];
            this.MoveToCoords(first.Latitude, first.Longitude);
        }
    }

    _RenderMarker(event) {
     
        const marker = L.marker([event.Latitude, event.Longitude])
            .addTo(this._map) 
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
                //EventDetailView.renderEventDetail(event);
                this.MoveToCoords(event.Latitude, event.Longitude);
                SidebarView.highlightEvent(event.EventId);
            })
             
        this._markers.push(marker);
        
    }

 

   
   
}

export default new MapView();