let hubUrl = 'https://localhost:44345/Comments';
const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(hubUrl)
    .configureLogging(signalR.LogLevel.Information)
    .build();

hubConnection.on("Send", function (message, userName, сommentId) {

    let elem = document.createElement("p");
    let div = document.createElement("div");
    div.innerHTML = " <div class=\"border rounded p - 3 mb - 3\"><h2 class=\"author\" >" + userName + "</h2 ><p class=\"text-context\">" + message + "</p><span>Rating: </span><span class=\"rating\">"+0+"</span><a class=\"like ml-3\"><img src=\"https://res.cloudinary.com/del5wrr12/image/upload/v1572829492/w2gh8giame3qj8q8smjp.png \" width=\"18\" /></a><a class=\"dislike ml-3\"><img src=\"https://res.cloudinary.com/del5wrr12/image/upload/v1572829492/veslqx4w2mepqah3br1q.png \" width=\"18\" /></a><input type=\"hidden\" class=\"comment-id\" value=\"" + сommentId +"\" /></div >";
    let like = div.querySelector(".like");
    let dislike = div.querySelector(".dislike");
    like.addEventListener("click", likeListener);
    dislike.addEventListener("click", dislikeListener);
    elem.appendChild(div);
    let firstChild = document.getElementById("comments-list").firstChild;
    document.getElementById("comments-list").insertBefore(elem, firstChild);

});

document.getElementById("sendBtn").addEventListener("click", function (e) {
    let message = document.getElementById("message").value;

    hubConnection.invoke("Send", message, userName, shirtId);
});

hubConnection.on('Rating', function (commentId, changeValue) {
    let comments = document.querySelectorAll(".comment-id");
    for (let i = 0; i < comments.length; i++) {
        if (comments[i].value == commentId)
            comments[i].parentNode.querySelector(".rating").textContent = parseInt(comments[i].parentNode.querySelector(".rating").textContent) + changeValue;
    }
});

let likes = document.querySelectorAll(".like");
let dislikes = document.querySelectorAll(".dislike");
let likeListener = function (e) {
    let comment = e.currentTarget.parentNode.querySelector(".comment-id").value;
    hubConnection.invoke('Rating', userName, 1, comment);
}
let dislikeListener = function (e) {
    let comment = e.currentTarget.parentNode.querySelector(".comment-id").value;
    hubConnection.invoke('Rating', userName, -1, comment);
}
for (let i = 0; i < likes.length; i++) {
    likes[i].addEventListener("click", likeListener);
    dislikes[i].addEventListener("click", dislikeListener);
}

hubConnection.start();