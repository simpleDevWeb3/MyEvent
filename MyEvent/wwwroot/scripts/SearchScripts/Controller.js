import * as Model from '../../scripts/Model.js'
import MapView from './Views/MapView.js';
import SidebarView from './Views/SidebarView.js';

import searchView from './Views/searchView.js';





const renderMap = async function () {
    try {
        const data = new URLSearchParams(window.location.search).get('q');
        await Model.getSearch(data);
        await Model.setAddress();
       

        console.log("load map");
        console.log(Model.state);

        MapView.render(Model.state);

        SidebarView.render(Model.state.Search);
    } catch (error) {
        console.log(error);
    }
}

const SearchController =  async function (data) {
   
   
    try {

       // Get search result from api
        await Model.getSearch(data);
        console.log(Model.state.Search);
        SidebarView.render(Model.state.Search)


       // const { Latitude: lat, Longitude: lng } = Model.state.Search[0];
        MapView._UpdateMarkers(Model.state.Search)
       // MapView.MoveToCoords(lat, lng);
        SidebarView.expand();
        
    } catch (error) {
        console.log(error);
    }
    
    
}
const HoverHandler = function (eventId) {
    const event = Model.state.Search.find(e => e.EventId === eventId);

    MapView.MoveToCoords(event.Latitude, event.Longitude);
    SidebarView.highlightEvent(event.EventId);
}










export const init = function () {
    //View
    renderMap(); 
    

    searchView.AddSearchHandler(SearchController);
    SidebarView.AddHover(HoverHandler);
  
}

