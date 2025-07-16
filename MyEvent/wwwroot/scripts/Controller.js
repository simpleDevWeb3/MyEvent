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
        
        

   
}

const SearchController =  async function (data) {

   
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

// Initiate GET request (AJAX-supported)
$(document).on('click', '[data-get]', e => {
    e.preventDefault();
    const url = e.target.dataset.get;
    location = url || location;
});

// Initiate POST request (AJAX-supported)
$(document).on('click', '[data-post]', e => {
    e.preventDefault();
    const url = e.target.dataset.post;
    const f = $('<form>').appendTo(document.body)[0];
    f.method = 'post';
    f.action = url || location;
    f.submit();
});

// Trim input
$('[data-trim]').on('change', e => {
    e.target.value = e.target.value.trim();
});

// Auto uppercase
$('[data-upper]').on('input', e => {
    const a = e.target.selectionStart;
    const b = e.target.selectionEnd;
    e.target.value = e.target.value.toUpperCase();
    e.target.setSelectionRange(a, b);
});

// RESET form
$('[type=reset]').on('click', e => {
    e.preventDefault();
    location = location;
});

// Check all checkboxes
$('[data-check]').on('click', e => {
    e.preventDefault();
    const name = e.target.dataset.check;
    $(`[name=${name}]`).prop('checked', true);
});

// Uncheck all checkboxes
$('[data-uncheck]').on('click', e => {
    e.preventDefault();
    const name = e.target.dataset.uncheck;
    $(`[name=${name}]`).prop('checked', false);
});

// Row checkable (AJAX-supported)
$(document).on('click', '[data-checkable]', e => {
    if ($(e.target).is(':input,a')) return;

    $(e.currentTarget)
        .find(':checkbox')
        .prop('checked', (i, v) => !v);
});

// Photo preview
$('.upload input').on('change', e => {
    const f = e.target.files[0];
    const img = $(e.target).siblings('img')[0];

    img.dataset.src ??= img.src;

    if (f && f.type.startsWith('image/')) {
        img.onload = e => URL.revokeObjectURL(img.src);
        img.src = URL.createObjectURL(f);
    }
    else {
        img.src = img.dataset.src;
        e.target.value = '';
    }

    // Trigger input validation
    $(e.target).valid();
});
