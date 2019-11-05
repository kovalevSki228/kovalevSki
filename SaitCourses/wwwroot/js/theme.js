function Theme() {
    if (localStorage.getItem("theme")) {
        var head = document.getElementsByTagName('head')[0];
        var link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = "/css/bright.css";
        head.appendChild(link);
        localStorage.removeItem("theme");
    } else {
        var head = document.getElementsByTagName('head')[0];
        var link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = "/css/dark.css";
        head.appendChild(link);
        localStorage.setItem("theme", "dark");

    }
};

window.onload = new function(){
    if (!localStorage.getItem("theme")) {
        var head = document.getElementsByTagName('head')[0];
        var link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = "/css/bright.css";
        head.appendChild(link);
    } else {
        var head = document.getElementsByTagName('head')[0];
        var link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = "/css/dark.css";
        head.appendChild(link);
    }
};