let hubUrl = 'https://localhost:44345/Comments';
const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(hubUrl)
    .configureLogging(signalR.LogLevel.Information)
    .build();

hubConnection.on("Send", function (message, userName) {

    let elem = document.createElement("p");
    let div = document.createElement("div");
    div.innerHTML = " <div class=\"border rounded p - 3 mb - 3\"><h2 class=\"author\" >" + userName + "</h2 ><p class=\"text-context\">" + message + "</p><span>Rating: </span><span class=\"rating\">Лайки</span><a class=\"like ml-3\"><img src=\"https://res.cloudinary.com/del5wrr12/image/upload/v1572829492/w2gh8giame3qj8q8smjp.png \" width=\"18\" /></a><a class=\"dislike ml-3\"><img src=\"https://res.cloudinary.com/del5wrr12/image/upload/v1572829492/veslqx4w2mepqah3br1q.png \" width=\"18\" /></a><input type=\"hidden\" class=\"comment-id\" value=\"Text\" /></div >";

    elem.appendChild(div);
    let firstElem = document.getElementById("chatroom").lastChild;
    document.getElementById("chatroom").insertBefore(elem, firstElem);

});

document.getElementById("sendBtn").addEventListener("click", function (e) {
    let message = document.getElementById("message").value;
    hubConnection.invoke("Send", message, userName);
});

hubConnection.start();