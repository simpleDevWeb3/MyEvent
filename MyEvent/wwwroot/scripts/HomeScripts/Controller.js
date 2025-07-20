import * as Model from "../../scripts/Model.js";
import CardView from "./Views/CardView.js";
import NavView from "./Views/NavView.js";
import CarouselView from "./Views/carouselView.js"
import LoadingSkleton from './Views/LoadingSkleton.js';
import TransitionLoading from './Views/TransitionLoading.js'


const ControllerEvent = async function () {

    LoadingSkleton.render();
 
    try {
        await Model.setAddress();
        await Model.getEvents();
        CardView.render(Model.state.Events);
    } catch (error) {
        console.log(error);
    } 
    
};

const ControllerCategory = async function (label) {
    //TransitionLoading.render();
    try {
        //$('.nav-lable').toggle();
        await Model.getByTags(label);
        $('.title-category').empty().append(label)
        CardView.render(Model.state.Events);
    } catch (error) {
        console.log(error);
        CardView._RenderError(error);
    }

};

export const init = function () {
    
    CardView.addHandlerDisplay(ControllerEvent);

    CarouselView.addHandleScroll();
    NavView.addHandlerHover();
    NavView.addHandlerClick(ControllerCategory);
};
