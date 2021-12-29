const apiList = document.querySelector(".api-options");
const beehiveList = document.querySelector(".beehive-options");
const queenList = document.querySelector(".queen-options");
const administrationList = document.querySelector(".administration-options");
const sidebarArrows = document.querySelectorAll(".primary .fas.fa-angle-left");

const apiButton = document.querySelector(".apiaries-drop-btn");
apiButton.addEventListener("click", (e) => {
    if (apiList.classList.contains("show")) {
        showHideMenu(
            true,
            apiList,
            document.querySelector(".apiaries-drop-btn .fa-angle-left")
        );
    } else {
        showHideMenu(
            false,
            apiList,
            document.querySelector(".apiaries-drop-btn .fa-angle-left")
        );
    }
});

const beehivesButton = document.querySelector(".beehives-drop-btn");
beehivesButton.addEventListener("click", (e) => {
    if (beehiveList.classList.contains("show")) {
        showHideMenu(
            true,
            beehiveList,
            document.querySelector(".beehives-drop-btn .fa-angle-left")
        );
    } else {
        showHideMenu(
            false,
            beehiveList,
            document.querySelector(".beehives-drop-btn .fa-angle-left")
        );
    }
});

const queensButton = document.querySelector(".queens-drop-btn");
queensButton.addEventListener("click", (e) => {
    if (queenList.classList.contains("show")) {
        showHideMenu(
            true,
            queenList,
            document.querySelector(".queens-drop-btn .fa-angle-left")
        );
    } else {
        showHideMenu(
            false,
            queenList,
            document.querySelector(".queens-drop-btn .fa-angle-left")
        );
    }
});

const administrationButton = document.querySelector(".administration-drop-btn");
if (administrationButton != null) {
    administrationButton.addEventListener("click", (e) => {
        if (administrationList.classList.contains("show")) {
            showHideMenu(
                true,
                administrationList,
                document.querySelector(".administration-drop-btn .fa-angle-left")
            );
        } else {
            showHideMenu(
                false,
                administrationList,
                document.querySelector(".administration-drop-btn .fa-angle-left")
            );
        }
    });
}

function showHideMenu(addOrRemove, list, arrow) {
    // backgroundToBase();
    removeClass([beehiveList, queenList, apiList, administrationList], "show");
    removeClass(sidebarArrows, "rotate-270");

    if (addOrRemove) {
        list.classList.remove("show");
        arrow.classList.remove("rotate-270");
    } else {
        list.classList.add("show");
        arrow.classList.add("rotate-270");
    }
    // button.style.background = btnBackground;
}

function removeClass(elements, className) {
    if (elements.length == 1) {
        elements.classList.remove(className);
    } else {
        elements.forEach((element) => {
            if (element != null) {
                element.classList.remove(className);
            }
        });
    }
}

var greetMessageElement = document.getElementById("greet-message");
var currentDateTime = new Date();

if (currentDateTime.getHours() < 12) {
    greetMessageElement.innerText = "Добро утро,";
} else if (currentDateTime.getHours() < 18) {
    greetMessageElement.innerText = "Добър ден,";
} else {
    greetMessageElement.innerText = "Добър вечер,";
}

const dataPicker = Array.from(document.querySelectorAll("#date-picker"));

dataPicker.forEach((date) => {
    date.value =
        currentDateTime.getDate() +
        "." +
        ("0" + (currentDateTime.getMonth() + 1)).slice(-2) +
        "." +
        ("0" + currentDateTime.getFullYear()).slice(-2);
});

// Get the element with id="defaultOpen" and click on it
// document.getElementById("defaultOpen").click();

function formSection(btn) {
    var btnId = btn.currentTarget.id.substr(3);
    var btn = document.getElementById("btn" + btnId);
    var div = document.getElementById(btnId);
    var checkBox = document.getElementById(
        "Include" + btnId.substr(0, btnId.length - 7)
    );

    if (div.style.display == "none") {
        div.style.display = "flex";
        checkBox.value = true;
        div.classList.remove("close-section");
        div.classList.add("open-section");
        btn.classList.add("section-button-clicked");
    } else {
        div.style.display = "none";
        checkBox.value = false;
        div.classList.remove("open-section");
        div.classList.add("close-section");
        btn.classList.remove("section-button-clicked");
    }
}

// expand table row
function expandTable(element, tab) {
    var masterId = element.id;
    var masterRow = document.getElementById(masterId);
    var expandId = masterId.replace("Master", "Expand");
    var expandRow = document.getElementById(expandId);

    // expand selected
    if (expandRow.style.display == "none") {
        expandRow.style.display = "";
        masterRow.classList.add("expanded");
    } else {
        expandRow.style.display = "none";
        masterRow.classList.remove("expanded");
    }
    
    // hide expanded
    var allExpandSelector = '*[id^="' + expandId.replace(/[0-9]/g, "") + '"]';
    var allExpandRows = document.querySelectorAll(allExpandSelector);
    hideExpanded(allExpandRows, expandRow);
}

// hide expanded row
function hideExpanded(allExpandRows, exeptRow) {
    allExpandRows.forEach((row) => {
        if (row != exeptRow) {
            row.style.display = "none";
        }
    });
}

// check form include sections
function checkFormSection(hiddenElementId) {
    var element = document.getElementById(hiddenElementId);
    var btnName = 'btn' + hiddenElementId.substr(7) + 'Section';

    if (element.value == 'True') {
        document.getElementById(btnName).click();
    }
}

function markRequiredFields() {
    var fields = document.querySelectorAll('input[data-val-required]');
    fields.forEach(field => {
        var label = field.parentElement.children[0];
        if (!label.classList.contains('back-arrow')) {
            label.classList.add("field-required");
        }
    });
}

function allBeehives() {
    var allBeehives = document.getElementById("AllBeehives");
    var beehiveNumbersSpaceSeparated = document.getElementById("BeehiveNumbersSpaceSeparated");
    if (allBeehives.value == true) {
        beehiveNumbersSpaceSeparated.style.display = "none";
    } else {
        beehiveNumbersSpaceSeparated.style.display = "block";
    }
}

function printQRCode() {
    var divContents = document.getElementById("qr-code-img").outerHTML;
    var a = window.open('', '', 'height=500, width=500');
    a.document.write('<html>');
    a.document.write('<body>');
    a.document.write('<div style="display: flex; flex-direction: column; align-items: center;">');
    a.document.write(divContents);
    a.document.write('<div>');

    var apiaryNumber = document.getElementById('apiary-num');
    if(apiaryNumber != null){
        a.document.write('Пчелин №' + apiaryNumber.innerText);
    }

    var beehiveNumber = document.getElementById('beehive-num');
    if(beehiveNumber != null){
        a.document.write(' / Кошер №' + beehiveNumber.innerText);
    }
    
    a.document.write('</div>');
    // a.document.write('<div>Пчелин №' + apiaryNumber +' / Кошер №' + 25 + '</div>');
    a.document.write('</div>');
    a.document.write('</body></html>');
    a.document.close();
    a.print();
}

var isAsideOpen = false;
function asideBurgerMenuClick() {
    var div = document.querySelector('.aside-icon');
    var i = document.querySelector('.aside-icon i');
    var aside = document.querySelector('.sidebar-navigation');
    var contentCover = document.querySelector('.aside-content-cover');
    
    if (isAsideOpen) {
        aside.style.display = "none";
        contentCover.style.display = "none";
        div.innerHTML = '<i class="fas fa-bars"></i>';
        isAsideOpen = false;
    } else {
        aside.style.display = "block";
        contentCover.style.display = "block";
        div.innerHTML = '<i class="fas fa-times"></i>';
        isAsideOpen = true;
    }
}

function closeAside(){ 
    asideBurgerMenuClick();
}

function ShowDeleteForm(id) {
    var deleteForm = document.getElementById(id);
    deleteForm.classList = 'delete-form-display-show';
}

function HideDeleteForm(id) {
    var deleteForm = document.getElementById(id);
    deleteForm.classList = 'delete-form-display';
}

const urlSearchParams = new URLSearchParams(window.location.search);
const params = Object.fromEntries(urlSearchParams.entries());
var property = params["orderBy"];
if (property) {
    document.getElementById(property).classList.add("ordered");
}