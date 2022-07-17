function showDropdown() {
  document.getElementById("dropdown-list").classList.add("show");
}

window.onclick = function (event) {
  if (!event.target.matches(".dropbtn .click")) {
    var dropdown = document.getElementsByClassName("dropdown-content");
    var i;
    for (i = 0; i < dropdown.length; i++) {
      var openDropdown = dropdown[i];
      if (openDropdown.classList.contains("show")) {
        openDropdown.classList.remove("show");
      }
    }
  }
};
