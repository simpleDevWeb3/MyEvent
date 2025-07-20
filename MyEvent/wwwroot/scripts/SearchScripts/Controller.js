import * as Model from '../../scripts/Model.js'
import MapView from './Views/MapView.js';
import SidebarView from './Views/SidebarView.js';
import EventDetailView from './Views/EventDetailView.js';
import searchView from './Views/searchView.js';





const renderMap = async function () {
    try {
        await Model.setAddress();
        await Model.getEvents();

        console.log("load map");


        MapView.render(Model.state);

        $('.locate__btn').addClass('hide');
        $('.user-location').empty().append(`Search Event in ${Model.state.address.city}`);
        $('.sidebar-small').removeClass('hide').css('display', 'flex');
        $('.home').removeClass('hide');
    } catch (error) {
        console.log(error);
    }
}

const SearchController =  async function (data) {
     if (window.location.pathname !== '/home/map') {
        window.location.href = '/home/map';
    }
   
    try {

       // Get search result from api
        await Model.getSearch(data);
        console.log(Model.state.Search);
        SidebarView.render(Model.state.Search)
        //Display search result
        $('.sidebar__header')
            .css('transform', 'translateX(0px)');

        $('.sidebar__events')
            .css('transform', 'translateX(0px)');

        $('.user-location')
            .empty()
            .append(`Search Event in ${data}`)



        const {Latitude:lat,Longitude:lng} = Model.state.Search[0];
        MapView.MoveToCoords(lat, lng);
        SidebarView.expand();
        
    } catch (error) {
        console.log(error);
    }
    
    
}

const EventDetailController = function (dataId) {
    const event = Model.state.Events.find(e => e.EventId === dataId) || Model.state.Search.find(e => e.EventId === dataId)
    SidebarView.highlightEvent(dataId);
    EventDetailView.renderEventDetail(event);
    MapView.MoveToCoords(event.Latitude,event.Longitude);    
  

}




const SidebarController = function () {
           SidebarView.render(Model.state.Events); // render navabar result
}





export const init = function () {
    //View
    renderMap(); 
    

    //UI Handling
    SidebarView.AddExpandSidebar(SidebarController);
    SidebarView.AddToggleSidebar(SidebarController);
   

    SidebarView.AddShowDetail(EventDetailController);
    EventDetailView.AddToggleDetail();

    searchView.AddSearchHandler(SearchController);
    searchView.AddfocusSearchInput();
    searchView.AddToggleSearchBar();
}

