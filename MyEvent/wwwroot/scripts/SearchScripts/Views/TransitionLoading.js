class TransitionLoading {
    _parentEl = $('.event--all')[0];


    render() {
        console.log('skleton')
         
         $(this._parentEl).append(this.renderLoading());
        $('.nav-lable').toggle();
    }

    renderLoading() {
        return `
         <div class="modals--loading">
                   
            </div>
        `
    }
}
export default new TransitionLoading();