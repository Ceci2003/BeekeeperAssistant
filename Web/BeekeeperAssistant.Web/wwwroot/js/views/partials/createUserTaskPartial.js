function chooseColor(element, color){
    var colors = document.querySelectorAll('#create-form .colors .color');

    colors.forEach(color => {
        color.classList.remove('active');
    });

    element.classList.add("active");

    console.log(colors);
    // console.log(element);

    document.getElementById("input-color").value = color;
}

function chooseEditColor(element, color){
    var colors = document.querySelectorAll('#edit-form .colors .color');

    colors.forEach(color => {
        color.classList.remove('active');
    });

    element.classList.add("active");

    console.log(colors);
    // console.log(element);

    document.getElementById("input-edit-color").value = color;
}

function closeEditForm() {
    document.getElementById("edit-form").style.display = "none";
    document.getElementById("create-form").style.display = "block";
}