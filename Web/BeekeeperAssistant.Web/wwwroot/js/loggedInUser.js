let apiList = document.querySelector(".api-options");
let beehiveList = document.querySelector(".beehive-options");
let queenList = document.querySelector(".queen-options");
let sidebarArrows = document.querySelectorAll(".fa-angle-left");

let apiButton = document.querySelector(".apiary-button");
apiButton.addEventListener("click", (e) => {

    if (!apiList.classList.contains("show")) {
        showMenu("add", apiList, apiButton, "#2B1C12", sidebarArrows[0]);
    }
    else {
        showMenu("remove", apiList, apiButton, "#F0BB00", sidebarArrows[0]);
    }
})

let beehiveButton = document.querySelector(".beehive-button");
beehiveButton.addEventListener("click", (e) => {

    if (!beehiveList.classList.contains("show")) {
        showMenu("add", beehiveList, beehiveButton, "#F0BB00", sidebarArrows[1]);
    }
    else {
        showMenu("remove", beehiveList, beehiveButton, "#2B1C12", sidebarArrows[1]);
    }
})


let queenButton = document.querySelector(".queen-button");
queenButton.addEventListener("click", (e) => {

    if (!queenList.classList.contains("show")) {
        showMenu("add", queenList, queenButton, "#F0BB00", sidebarArrows[2]);
    }
    else {
        showMenu("remove", queenList, queenButton, "#2B1C12", sidebarArrows[2]);
    }
})

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
    button.style.background = btnBackground;
}

function removeClass(elements, className) {
    elements.forEach(element => {
        element.classList.remove(className);
    });
}

function backgroundToBase() {
    queenButton.style.background = "#2B1C12";
    beehiveButton.style.background = "#2B1C12";
    apiButton.style.background = "#2B1C12";
}