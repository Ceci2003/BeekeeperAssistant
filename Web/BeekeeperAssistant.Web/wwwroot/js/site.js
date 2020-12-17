function passwordShowHide() {
    const input = document.querySelector('#input-password');
    const eye = document.querySelector('#password-eye');
    if (input.type == 'password') {
        input.type = 'text';
        eye.innerHTML = '<i class="fas fa-eye-slash"></i>';
    } else {
        input.type = 'password';
        eye.innerHTML = '<i class="fas fa-eye"></i>';
    }
}

function rePasswordShowHide() {
    const input = document.querySelector('#input-re-password');
    const eye = document.querySelector('#re-password-eye');
    if (input.type == 'password') {
        input.type = 'text';
        eye.innerHTML = '<i class="fas fa-eye-slash"></i>';
    } else {
        input.type = 'password';
        eye.innerHTML = '<i class="fas fa-eye"></i>';
    }
}


// if(document.getElementsByClassName("field-validation-error").length != 0)
// {
//     document.querySelectorAll(".submit-btn")[0].disabled=true;
// }
// else
// {
//     document.querySelectorAll(".submit-btn")[0].disabled=false
// }