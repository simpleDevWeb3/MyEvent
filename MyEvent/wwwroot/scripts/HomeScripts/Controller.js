import * as Model from "../../scripts/Model.js";
import MoreView from "./Views/MoreView.js";
import NavView from "./Views/NavView.js";
import CarouselView from "./Views/carouselView.js"
const ControllerEvent = async function () {
    try {
        await Model.setAddress();
        await Model.getEvents();
        MoreView.render(Model.state.Events);
    } catch (error) {
        console.log(error);
    }
    
};

const ControllerCategory = async function (label) {
    try {
        await Model.getByCategory();
        $('.title-category').empty().append(label)
        MoreView.render(Model.state.Events);
    } catch (error) {
        console.log(error);
    }

};
export const init = function () {
    
    MoreView.addHandlerDisplay(ControllerEvent);
    CarouselView.addHandleScroll();
    NavView.addHandlerHover();
    NavView.addHandlerClick(ControllerCategory);
};
