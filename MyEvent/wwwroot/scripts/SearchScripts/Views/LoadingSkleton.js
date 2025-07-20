class LoadingSkleton {
    _parentEl = $('.event--all')[0];
    _cardNumber = 8;

    render() {
        console.log('skleton')
    
        $(this._parentEl).empty()
       for (let i = 0; i < this._cardNumber; i++) { 
           $(this._parentEl).append(this.renderLoading());
        }
    }

    renderLoading(){
        return`
         <div class="home-event">
                    <div class="card-image ">
                        <div class="home-event-img skleton--img loading">
                        </div>
                    </div>

                    <div class="home-event-title skleton--title loading"></div>
                    <div class="skleton--date loading">
                    </div>

                </div>
        `
    }
}
export default new LoadingSkleton();