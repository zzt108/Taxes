const app = document.getElementById('root');

function UserGetAction() {
    // alert("UserGetAction-Entry");
        var xhttp = new XMLHttpRequest();
    //  xhttp.onreadystatechange = function() {
    //      if (this.readyState == 4 && this.status == 200) {
    //          alert(this.responseText);
    //      }
    //  };
    //  alert("UserGetAction-GET");
        xhttp.open("GET", "http://localhost:8080/tax/vilnius/2016/1/1", true);
        xhttp.setRequestHeader("Content-type", "application/json");
        xhttp.onload = function () {

        // alert("UserGetAction-ONLoad");
        // Begin accessing JSON data here
        var data = JSON.parse(this.response);
        if (xhttp.status >= 200 && xhttp.status < 400) {
        data.forEach(movie => {
            const card = document.createElement('div');
            card.setAttribute('class', 'card');

            const h1 = document.createElement('h1');
            h1.textContent = movie.title;

            const p = document.createElement('p');
            movie.description = movie.description.substring(0, 300);
            p.textContent = `${movie.description}...`;

            container.appendChild(card);
            card.appendChild(h1);
            card.appendChild(p);
        });
        } else {
            const errorMessage = document.createElement('marquee');
            errorMessage.textContent = `Gah, it's not working!`;
            app.appendChild(errorMessage);
        }
        }
        xhttp.onerror = function(){
        const errorMessage = document.createElement('marquee');
        errorMessage.textContent = `Gah, No API`;
        app.appendChild(errorMessage);
        }
        xhttp.send();

}
