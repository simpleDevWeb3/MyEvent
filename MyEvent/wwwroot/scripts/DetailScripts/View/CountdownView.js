class CountdownView {
    _parentEl = $('.Countdown')[0];
    _data;
    _day; 
    _hours;
    _mins;
    _secs;
    render(data) {
        this._data = data;
        this._countdown();
       
    }
    _countdown() {
        setInterval(() => {
            this._updateCountDown();
            $(this._parentEl).html(
                        `<div>
                            <div>${this._day}</div>
                            <div style="font-size:15px;">Days</div>
                        </div>
                        <div>
                            <div>${this._hours}</div>
                            <div style="font-size:15px;">Hours</div>
                        </div>
                        <div>
                            <div>${this._mins}</div>
                            <div style="font-size:15px;">Minutes</div>
                        </div>
                        <div>
                            <div>${this._secs}</div>
                            <div style="font-size:15px;">Seconds</div>
                        </div>`)
         }
          , 1000);
    }

    _updateCountDown() {

        // Get current 
        const today = dayjs();


        //Get Event Date
        const EndDateObj = this._data.Date;
        const EndDate = dayjs(EndDateObj);


        //get range of today and enddate in seconds
        const diffInHours = EndDate.diff(today, 'hour');
        const diffInSecs = EndDate.diff(today, 'second');
        console.log(diffInHours);

        this._day = Math.floor(diffInHours / 24);
        this._hours = diffInHours % 24;
        this._mins = Math.floor((diffInSecs % 3600) / 60);
        this._secs = Math.floor(diffInSecs % 60);

      
    }
}

export default new CountdownView();