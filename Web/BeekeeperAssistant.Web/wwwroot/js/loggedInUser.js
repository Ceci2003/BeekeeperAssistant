let apiList = document.querySelector(".api-options");
let beehiveList = document.querySelector(".beehive-options");
let queenList = document.querySelector(".queen-options");

let apiButton = document.querySelector(".apiary-button");
apiButton.addEventListener("click", (e) => {

    if (!apiList.classList.contains("show")) {
        showMenu("add", apiList, apiButton, "#F0BB00");
    }
    else {
        showMenu("add", apiList, apiButton, "#2B1C12");
    }
})

let beehiveButton = document.querySelector(".beehive-button");
beehiveButton.addEventListener("click", (e) => {

    if (!beehiveList.classList.contains("show")) {
        showMenu("add", beehiveList, beehiveButton, "#F0BB00");
    }
    else {
        showMenu("add", beehiveList, beehiveButton, "#2B1C12");
    }
})


let queenButton = document.querySelector(".queen-button");
queenButton.addEventListener("click", (e) => {

    if (!queenList.classList.contains("show")) {
        showMenu("add", queenList, queenButton, "#F0BB00");
    }
    else {
        showMenu("add", queenList, queenButton, "#2B1C12");
    }
})

function showMenu(addOrRemove, list, button, btnBackground) {
    backgroundToBase();
    removeClass([beehiveList, queenList, apiList], "show");
    if (addOrRemove == "add") {
        list.classList.add("show");
    }
    else if (addOrRemove == "remove") {
        list.classList.remove("show");
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