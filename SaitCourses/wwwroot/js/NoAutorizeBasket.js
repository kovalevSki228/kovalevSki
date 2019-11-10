function addShirtBasket(shirtName, data)
{
    if (localStorage.getItem("Col")) {

        col = +localStorage.getItem("Col") + 1;
        localStorage.setItem("Shirt name " + col, shirtName);
        let size = document.getElementById("size").value;
        localStorage.setItem("Shirt size " + col, size);
        let sex = document.getElementById("sex").value;
        localStorage.setItem("Shirt sex " + col, sex);
        localStorage.setItem("Shirt data " + col, data);
        localStorage.setItem("Col", col);
    }
    else {
        localStorage.setItem("Col", 0);
        col = +localStorage.getItem("Col") + 1;
        localStorage.setItem("Shirt name " + col, shirtName);
        let size = document.getElementById("size").value;
        localStorage.setItem("Shirt size " + col, size);
        let sex = document.getElementById("sex").value;
        localStorage.setItem("Shirt sex " + col, sex);
        localStorage.setItem("Shirt data " + col, data);
        localStorage.setItem("Col", col);
    }
}

function getShirtBasket() {

    var message ="";
    var t = "";
    col = +localStorage.getItem("Col");
    for (var i = 1; i <= col; i++) {
        if (localStorage.getItem("Shirt name " + i)) {
            var tr = "<tr>";
            tr += "<td type=\"text\" name=\"@Modal[" + (i - 1) + "].nameShirt\" asp-for=\"@Modal[" + (i - 1) + "].nameShirt\">" + localStorage.getItem("Shirt name " + i); + "</td>";
            tr += "<td>" + 1 + "</td>";
            tr += "<td type=\"text\" name=\"@Modal[" + (i - 1) + "].size\" asp-for=\"@Modal[" + (i - 1) + "].size\">" + localStorage.getItem("Shirt size " + i); + "</td>";
            tr += "<td type=\"text\" name=\"@Modal[" + (i - 1) + "].sex\" asp-for=\"@Modal[" + (i - 1) + "].sex\">" + localStorage.getItem("Shirt sex " + i); + "</td>";
            tr += "<td type=\"text\" name=\"@Modal[" + (i - 1) + "].dataOfPurchase\"  asp-for=\"@Modal[" + (i - 1) + "].dataOfPurchase\">" + localStorage.getItem("Shirt data " + i); + "</td>";
            tr += "<td> <button id=\"DeletePurchase\" onclick=\"DeletePurchaseBtn(" + i + ")\" style=\"display: inline - block\" type=\"submit\" class=\"btn btn - sm btn - outline - danger\"> Delete</button ></td>";
            tr += "</tr>";
            message += localStorage.getItem("Shirt name " + i) + ", size: " + localStorage.getItem("Shirt size " + i) + ", sex: " + localStorage.getItem("Shirt sex " + i) + ", price: 30$; ";
            t += tr;
        }
    }
    document.getElementById("messadeBasked").value = message;
    document.getElementById("basketTable").innerHTML += t;
}

function DeletePurchaseBtn(i) {
    localStorage.removeItem("Shirt name " + i);
    localStorage.removeItem("Shirt size " + i);
    localStorage.removeItem("Shirt sex " + i);
    localStorage.removeItem("Shirt data " + i);
}

function clearBasket() {
    col = +localStorage.getItem("Col");
    for (var i = 1; i <= col; i++) {
        localStorage.removeItem("Shirt name " + i);
        localStorage.removeItem("Shirt size " + i);
        localStorage.removeItem("Shirt sex " + i);
        localStorage.removeItem("Shirt data " + i);
        localStorage.removeItem("Col");
    }
}

