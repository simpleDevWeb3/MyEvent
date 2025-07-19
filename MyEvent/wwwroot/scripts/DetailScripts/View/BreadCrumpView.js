

class BreadCrumpView {
    _parentEL = $('.breadCrump')[0];
    
    render() {
        const urlPath = window.location.pathname
            .split('/')
            .filter(Boolean)
            .map(part => decodeURIComponent(part).replace(/%/g, '').trim());

        console.log(urlPath);

        let accumulatedPath = '';
        const lastPath = urlPath.length - 1;
        let markup = '';

        console.log(lastPath);

        urlPath.forEach((part,i) => {

            if (i == lastPath) {
                markup += `<span > > ${part}</span>`;
                return;
            }
                
            accumulatedPath += `/${encodeURIComponent(part)}`;
            markup += this._Markup(part, accumulatedPath)
           
        });
        $(this._parentEL).append(markup);
    }

    _Markup(part, accumulatedPath) {

        return `
        <a href="${accumulatedPath}">${part}</a>
    `;
    }
}

export default new BreadCrumpView();