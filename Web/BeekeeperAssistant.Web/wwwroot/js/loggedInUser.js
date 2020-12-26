const apiList = document.querySelector(".api-options");
const beehiveList = document.querySelector(".beehive-options");
const queenList = document.querySelector(".queen-options");
const sidebarArrows = document.querySelectorAll(".fa-angle-left");

const apiButton = document.querySelector(".apiary-button");
apiButton.addEventListener("click", (e) => {

    if (!apiList.classList.contains("show")) {
        showMenu("add", apiList, apiButton, "#FF7800", sidebarArrows[1]);
    }
    else {
        showMenu("remove", apiList, apiButton, "#110400", sidebarArrows[1]);
    }
})

const beehiveButton = document.querySelector(".beehive-button");
beehiveButton.addEventListener("click", (e) => {

    if (!beehiveList.classList.contains("show")) {
        showMenu("add", beehiveList, beehiveButton, "#FF7800", sidebarArrows[2]);
    }
    else {
        showMenu("remove", beehiveList, beehiveButton, "#110400", sidebarArrows[2]);
    }
})


const queenButton = document.querySelector(".queen-button");
queenButton.addEventListener("click", (e) => {

    if (!queenList.classList.contains("show")) {
        showMenu("add", queenList, queenButton, "#FF7800", sidebarArrows[3]);
    }
    else {
        showMenu("remove", queenList, queenButton, "#110400", sidebarArrows[3]);
    }
})


const dataPicker = document.querySelector("#dataPick");
dataPicker?.addEventListener("click", (e) => {
    var today = new Date();
    dataPicker.value = today.getFullYear() + '-' + ('0' + (today.getMonth() + 1)).slice(-2) + '-' + ('0' + today.getDate()).slice(-2);
})

//  Functions

function showMenu(addOrRemove, list, button, btnBackground, arrow) {
    backgroundToBase();
    removeClass([beehiveList, queenList, apiList], "show");
    removeClass(sidebarArrows, "rotate-90");
    if (addOrRemove == "add") {
        list.classList.add("show");
        arrow.classList.add("rotate-90");
    }
    else if (addOrRemove == "remove") {
        list.classList.remove("show");
        arrow.classList.remove("rotate-90");
    }
    // button.style.background = btnBackground;
}

function removeClass(elements, className) {
    elements.forEach(element => {
        element.classList.remove(className);
    });
}

function backgroundToBase() {
    queenButton.style.background = "#110400";
    beehiveButton.style.background = "#110400";
    apiButton.style.background = "#110400";
}
