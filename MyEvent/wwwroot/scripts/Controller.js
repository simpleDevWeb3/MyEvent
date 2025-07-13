import * as Model from './Model.js';
import NavBarView from './Views/NavBarView.js';
import MainView from './Views/MainView.js';
import MapView from './Views/MapView.js';
import SidebarView from './Views/SidebarView.js';
import EventDetailView from './Views/EventDetailView.js';
import searchView from './Views/searchView.js';

const HomeController= function () {
    $('#map').toggleClass('hide');
    $('.Main-view').toggleClass('hide');
    SidebarView.toggle();
    EventDetailView.toggleEventDetail();
    $('.sidebar-small')
        .toggleClass('hide')
        .css('display', 'none');

}

const MainViewController = async function () {

    try {

        await Model.setAddress(); //SET Current Position
        await Model.getEvents();  // SET Events Obj
      

       
        console.log(Model.state);
      
    } catch (error) {
        console.log(error)
    }
}
const MapController = async function () {
    //$('.form-locate').empty().append(' <div class="lds-heart"><div></div></div>')
    $('#map').toggleClass('hide');

    try {
     
        
        MapView.render(Model.state);
        SidebarView.render(Model.state.Events); // render navabar result
        $('.locate__btn').addClass('hide');
    
        $('.user-location')
            .empty()
            .append(`Search Event in ${Model.state.address.city}`)

        $('.sidebar-small')
            .toggleClass('hide')
            .css('display', 'flex');
        $('.locate__btn').addClass('hide');
        $(".home").toggleClass('hide')
        
        

    } catch (error) {
        console.log(error)
    }
}

const SearchController =  async function (data) {

    $('.sidebar__header')
        .css('transform', 'translateX(0px)');

    $('.sidebar__events')
        .css('transform', 'translateX(0px)');

    $('.user-location')
        .empty()
        .append(`Search Event in ${data}`)
    SidebarView.expand();
    try {
       // Get search result from api
        await Model.getSearch(data);
        console.log(Model.state.Search);
        SidebarView.render(Model.state.Search)
        //Display search result
       
        
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
    console.log('hi')
}




export const init = function () {
    NavBarView.AddHandlerOpenMap(MapController);
    NavBarView.AddHandlerHome(HomeController);

    MainView.addHandlerLocate(MainViewController);

    SidebarView.AddExpandSidebar(SidebarController);
    SidebarView.AddToggleSidebar(SidebarController);
   

    SidebarView.AddShowDetail(EventDetailController);
    EventDetailView.AddToggleDetail();

    searchView.AddSearchHandler(SearchController);
    searchView.AddfocusSearchInput();
    searchView.AddToggleSearchBar();
}