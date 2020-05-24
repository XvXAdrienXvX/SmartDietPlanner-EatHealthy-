var myInput = document.getElementById("pwdInput");
var letter = document.getElementById("letter");
var capital = document.getElementById("capital");
var number = document.getElementById("number");
var length = document.getElementById("length");

//enable and disable password tooltip
$("#pwdInput").focus(function () {
    $("#PwdTip").css("display", "block");
    validateInp();

});

$("#pwdInput").blur(function () {

    $("#PwdTip").css("display", "none");

    

});
////End


// When the user starts to type something inside the password field
myInput.onkeyup = function () {
    validateInp();
};



function validateInp() {
    // Validate lowercase letters
    var lowerCaseLetters = /[a-z]/g;
    if (myInput.value.match(lowerCaseLetters)) {
        letter.classList.remove("fa-times");
        letter.classList.add("fa-check");
        $("#letter").css("color", "green");


    } else {
        letter.classList.remove("fa-check");
        letter.classList.add("fa-times");
        $("#letter").css("color", "red");
    }

    // Validate capital letters
    var upperCaseLetters = /[A-Z]/g;
    if (myInput.value.match(upperCaseLetters)) {
        capital.classList.remove("fa-times");
        capital.classList.add("fa-check");
        $("#capital").css("color", "green");


    } else {
        capital.classList.remove("fa-check");
        capital.classList.add("fa-times");
        $("#capital").css("color", "red");
    }
    // Validate numbers
    var numbers = /[0-9]/g;
    if (myInput.value.match(numbers)) {
        number.classList.remove("fa-times");
        number.classList.add("fa-check");
        $("#number").css("color", "green");


    } else {
        number.classList.remove("fa-check");
        number.classList.add("fa-times");
        $("#number").css("color", "red");
    }

    // Validate length
    if (myInput.value.length >= 8) {
        length.classList.remove("fa-times");
        length.classList.add("fa-check");
        $("#length").css("color", "green");


    } else {
        length.classList.remove("fa-check");
        length.classList.add("fa-times");
        $("#length").css("color", "red");
    }


}

$(document).ready(function () {
    $("#show_hide_password").on('click', function (event) {
        event.preventDefault();
        if ($('#pwdInput').attr("type") == "text") {
            $('#pwdInput').attr('type', 'password');
            $('#cpass').attr('type', 'password');
            $('#show_hide_password i').addClass("fa-eye-slash");
            $('#show_hide_password i').removeClass("fa-eye");
        } else if ($('#pwdInput').attr("type") == "password") {
            $('#pwdInput').attr('type', 'text');
            $('#cpass').attr('type', 'text');
            $('#show_hide_password i').removeClass("fa-eye-slash");
            $('#show_hide_password i').addClass("fa-eye");
        }
    });

});