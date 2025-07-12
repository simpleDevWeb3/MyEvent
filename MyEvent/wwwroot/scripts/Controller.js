import * as Model from './Model.js';
import MapView from './Views/MapView.js';
import SidebarView from './Views/SidebarView.js';
import EventDetailView from './Views/EventDetailView.js';

import searchView from './Views/searchView.js';

const MapController = async function () {
    $('.form-locate').empty().append(' <div class="lds-heart"><div></div></div>')
    try {

        await Model.setAddress(); //SET Current Position
        await Model.getEvents();  // SET Events Obj
        console.log(Model.state);
        
        MapView.render(Model.state);
        SidebarView.render(Model.state.Events); // render navabar result

        $('.user-location')
            .empty()
            .append(`Search Event in ${Model.state.address.city}`)

        $('.sidebar-small')
            .toggleClass('hide')
            .css('display', 'flex');
        $('.locate__btn').addClass('hide');
        $('.form-locate').empty()

        

    } catch (error) {
        console.log(error)
    }
}

const SearchController = async function (data) {

    $('.sidebar__header')
        .css('transform', 'translateX(0px)');

    $('.sidebar__events')
        .css('transform', 'translateX(0px)');

    $('.user-location')
        .empty()
        .append(`Search Event in ${data}`)


     
    SidebarView.expand();
}

const EventDetailController = function (dataId) {
    const event = Model.state.Events.find(e => e.EventId === dataId)
    SidebarView.highlightEvent(dataId);
    EventDetailView.renderEventDetail(event);
    MapView.MoveToCoords(event.Latitude,event.Longitude);    
  

}




const SidebarController = function () {
    MapView._ResizeObserver();
}




export const init = function () {

    MapView.addHandlerLocate(MapController);

    SidebarView.AddExpandSidebar(SidebarController);
    SidebarView.AddToggleSidebar(SidebarController);
   

    SidebarView.AddShowDetail(EventDetailController);
    EventDetailView.AddToggleDetail();

    searchView.AddSearchHandler(SearchController);
    searchView.AddfocusSearchInput();
    searchView.AddToggleSearchBar();
}