import * as Model from '../../scripts/Model.js'
import MapView from './Views/MapView.js';
import SidebarView from './Views/SidebarView.js';
import EventDetailView from './Views/EventDetailView.js';
import searchView from './Views/searchView.js';





const renderMap = async function () {
    try {
        const data = new URLSearchParams(window.location.search).get('q');
        await Model.getSearch(data);
        await Model.setAddress();
       

        console.log("load map");
        console.log(Model.state);

        MapView.render(Model.state);

        $('.locate__btn').addClass('hide');
        $('.user-location').empty().append(`Search Event in ${Model.state.address.city}`);
        $('.sidebar-small').removeClass('hide').css('display', 'flex');
        $('.home').removeClass('hide');
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
        MapView._UpdateMarkers();
       // MapView.MoveToCoords(lat, lng);
        SidebarView.expand();
        
    } catch (error) {
        console.log(error);
    }
    
    
}











export const init = function () {
    //View
    renderMap(); 
    

    //UI Handling
    //SidebarView.AddExpandSidebar(SidebarController);
   // SidebarView.AddToggleSidebar(SidebarController);
   

    //SidebarView.AddShowDetail(EventDetailController);
    //EventDetailView.AddToggleDetail();

    searchView.AddSearchHandler(SearchController);
  
  
}

