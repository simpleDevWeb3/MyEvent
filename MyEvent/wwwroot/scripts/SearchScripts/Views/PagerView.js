class PagerView {
    _ParentEl = $('.pagination')[0];

    render(data) {
        this._data = data;
        $(this._ParentEl).empty().append(this._generateMarkup());
    }

    _generateMarkup() {
        const PageLimit = Math.ceil(this._data.Search.length / this._data.Paging.ResultPerPage);
        const currentIndex = this._data.Paging.Page;
        console.log(currentIndex);
        console.log(PageLimit);
        if (PageLimit === 1) return;
  
        if (currentIndex > PageLimit) return;
        //first page generate next btn
        if (currentIndex === 1 && PageLimit > 1) {
           return this._generateNextBtn(currentIndex);
        }
         //Final Page generate prev btn
        else if (currentIndex === PageLimit) {
            return this._generatePrevBtn(currentIndex);
        }
        // middle page generate both btn
        else {
            return [this._generatePrevBtn(currentIndex),this._generateNextBtn(currentIndex)];
        }
    }

    _generateNextBtn(currentIndex) {
        return `
        <button class="home-button  paging__nextBtn" data-goto = "${currentIndex + 1}" style="font-size:15px; float:right;">
                <div>Page ${currentIndex + 1}</div>
          </button>
        `
    }

    _generatePrevBtn(currentIndex) {
        return `
            <button class="home-button paging__prevBtn" " data-goto = "${currentIndex - 1}" style="font-size:15px; float:left;">
                <div>Page ${currentIndex - 1}</div>
            </button>
            `
    }

    addHandlerPager(handler) {
        $(this._ParentEl).on('click', '[data-goto]', (e) => {
            console.log(e.currentTarget)

            //Get page
            const page = +e.currentTarget.dataset.goto;

            //trigger event
            handler(page);

            
        })
    }

}

export default new PagerView();