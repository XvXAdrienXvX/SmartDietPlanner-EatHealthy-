
//jQuery time
var current_fs, next_fs, previous_fs; //fieldsets
var left, opacity, scale; //fieldset properties which we will animate
var animating; //flag to prevent quick multi-click glitches
var NotInuseUsername;
var PrevUsername = "";
var formNumber = 1;
var proceedToNextForm;
function hello() {
    alert("as");
}

function checkusername(callback) {
    var username = document.getElementById("username").value;
    if (username != PrevUsername) {
        PrevUsername = username;
        var spin = document.getElementById("RegisterSpinner");
        spin.innerHTML = " <div></div> <div></div> <div></div> <div></div>";

        $.ajax({
            type: "get",
            url: "/User/VerifyUsername",
            data: { Username: username },
            success: function (message) {
                if (message == "true") {
                    NotInuseUsername = true;
                    callback();
                }
                else {
                    NotInuseUsername = false;

                    callback();
                }
            },
            error: function () {
                alert("error! try reloading the page");
            }
        });
    }
    else {
        callback();
    }
        
}
//prevent user from entering invalid characters
$('.RestrictTxt').keypress(function (e) {
    var regex = new RegExp("^[a-zA-Z0-9]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (!regex.test(str)) {

        e.preventDefault();
    }
});

function ValidateEmail(mail) {
    var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    if (mail.match(mailformat))
        return true;
    else
    return false;
}


function SubmitForm() {
    document.getElementById("err3").innerHTML = "";

    proceedToNextForm = true;
    var x = document.getElementsByClassName("ThirdFormInputs");


    for (i = 0; i < x.length; i++) {
        if (x[i].value == "" || x[i].value == " ") {
            x[i].style.border = "2px solid red";
            proceedToNextForm = false;
            document.getElementById("err3" ).innerHTML = "<div class='alert alert-danger'><strong> Error </strong> Please fill all the fields to proceed  </div>";
        }

        else {
            x[i].style.border = "none";
        }


    }

    if (proceedToNextForm == true) {
        document.getElementById("msform").submit();

    }

}

function ValidatePwd() {
    if ($('.fa-times')[0]) {
        
        return false;
    }
    else {
        return true;
    }}

$(".next").click(function () {
    proceedToNextForm = true;
    var thisElement = $(this);
    document.getElementById("err" + formNumber).innerHTML = "";
    var pwd = document.getElementsByName("UserPassword")[0].value;
    var cpwd = document.getElementsByName("cpass")[0].value;
    var x;
    var i;
    var passwordValid;

    if (formNumber == 1) {
        x = document.getElementsByClassName("FirstFormInputs");
    }
    else if (formNumber == 2) {
        x = document.getElementsByClassName("SecondFormInputs");
    }
    else {
        x = document.getElementsByClassName("ThirdFormInputs");

    }
    for (i = 0; i < x.length; i++) {
        if (x[i].value == "" || x[i].value == " ") {
            x[i].style.border = "2px solid red";
            proceedToNextForm = false;
            document.getElementById("err" + formNumber).innerHTML = "<div class='alert alert-danger'><strong> Error </strong> Please fill all the fields to proceed  </div>";
        }

        else if (x[i].type == "email") {
            if (ValidateEmail(x[i].value) == false) {
                document.getElementById("MailErr").innerHTML = "<div class='alert alert-danger'><strong> Error </strong> Please enter a valid email address </div>";
                proceedToNextForm = false;
                x[i].style.border = "2px solid red";

            }
            else {
                document.getElementById("MailErr").innerHTML = "";
                x[i].style.border = "none";


            }
           
        }
        else if (x[i].name == "UserAge") {
            if (x[i].value < 18 || x[i].value > 80) {
                document.getElementById("AgeErr").innerHTML = "<div class='alert alert-danger'><strong> Error </strong> Age has to be between 18-80 </div>";
                proceedToNextForm = false;
                x[i].style.border = "2px solid red";

            }
            else {
                document.getElementById("AgeErr").innerHTML = "";
                x[i].style.border = "none";


            }
        }
        else {
            x[i].style.border = "none";

        }

       
    }
    var UserN = document.getElementById("username");
    var CorrectUsernameLength;
    if (UserN.value.length < 8) {
        CorrectUsernameLength = false;
        UserN.style.border = "2px solid red";
        document.getElementById("UsernameErr").innerHTML = "<div class='alert alert-danger'><strong> Error </strong> Please enter a username longer than 8 characters </div>";


    }
    else {
        CorrectUsernameLength = true;
        UserN.style.border = "none";
        document.getElementById("UsernameErr").innerHTML = "";
    }
    passwordValid = ValidatePwd();
    if (passwordValid == false) {
        document.getElementById("PwdErr").innerHTML = "<div class='alert alert-danger'><strong> Error </strong> Please enter a correct password format  </div>";
        $("#PwdTip").css("display", "block");
        document.getElementById("pwdInput").style.border = "2px solid red";

    }


    else if (pwd != cpwd) {
                    document.getElementById("PwdErr").innerHTML = "<div class='alert alert-danger'><strong> Error </strong> Password Don't Match </div>";
        document.getElementsByName("UserPassword")[0].style.border = "2px solid red";
        document.getElementsByName("cpass")[0].style.border = "2px solid red";

    }

    else {
            document.getElementById("PwdErr").innerHTML = "";
            document.getElementById("pwdInput").style.border = "none";

        
    }

    if (CorrectUsernameLength == true) {
        if (passwordValid == true) {
            if (pwd == cpwd) {
                document.getElementById("pwdInput").innerHTML = "<div class='alert alert-danger'><strong> Error </strong> Username is taken  </div>";
                document.getElementsByName("UserPassword")[0].style.border = "none";
                document.getElementsByName("cpass")[0].style.border = "none";
                if (proceedToNextForm == true) {
                
                    checkusername(function () {
                        var spin = document.getElementById("RegisterSpinner");
                        spin.innerHTML = "";

                        if (NotInuseUsername == true) {

                            document.getElementById("err" + formNumber).innerHTML = "";
                            UserN.style.border = "None";
                            document.getElementById("UsernameErr").innerHTML = "";

                            if (animating) return false;
                            animating = true;

                            current_fs = thisElement.parent();
                            next_fs = thisElement.parent().next();

                            //activate next step on progressbar using the index of next_fs
                            $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");
                            formNumber = formNumber + 1;
                            //show the next fieldset
                            next_fs.show();
                            //hide the current fieldset with style
                            current_fs.animate({ opacity: 0 }, {
                                step: function (now, mx) {
                                    //as the opacity of current_fs reduces to 0 - stored in "now"
                                    //1. scale current_fs down to 80%
                                    scale = 1 - (1 - now) * 0.2;
                                    //2. bring next_fs from the right(50%)
                                    left = (now * 50) + "%";
                                    //3. increase opacity of next_fs to 1 as it moves in
                                    opacity = 1 - now;
                                    current_fs.css({
                                        'transform': 'scale(' + scale + ')',
                                        'position': 'absolute'
                                    });
                                    next_fs.css({ 'left': left, 'opacity': opacity });
                                },
                                duration: 800,
                                complete: function () {
                                    current_fs.hide();
                                    animating = false;
                                },
                                //this comes from the custom easing plugin
                                easing: 'easeInOutBack'
                            });

                        }
                        else {
                            document.getElementById("UsernameErr").innerHTML = "<div class='alert alert-danger'><strong> Error </strong> Username is taken  </div>";
                            UserN.style.border = "2px solid red";


                        }
                    });
                }
              
            }
        
        }
       
    }
   
   


});


$(".previous").click(function () {

    formNumber = formNumber - 1;
	if(animating) return false;
	animating = true;
	
	current_fs = $(this).parent();
	previous_fs = $(this).parent().prev();
	
	//de-activate current step on progressbar
	$("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");
	
	//show the previous fieldset
	previous_fs.show(); 
	//hide the current fieldset with style
	current_fs.animate({opacity: 0}, {
		step: function(now, mx) {
			//as the opacity of current_fs reduces to 0 - stored in "now"
			//1. scale previous_fs from 80% to 100%
			scale = 0.8 + (1 - now) * 0.2;
			//2. take current_fs to the right(50%) - from 0%
			left = ((1-now) * 50)+"%";
			//3. increase opacity of previous_fs to 1 as it moves in
			opacity = 1 - now;
			current_fs.css({'left': left});
			previous_fs.css({'transform': 'scale('+scale+')', 'opacity': opacity});
		}, 
		duration: 800, 
		complete: function(){
			current_fs.hide();
			animating = false;
		}, 
		//this comes from the custom easing plugin
		easing: 'easeInOutBack'
	});
});

$(".submit").click(function () {
    return false;
});
var InputFilled;
function validationFunction() {
    InputFilled = true;
    $('input').each(function () {
        var inpVal = ($(this).val());
        if (inpVal == null || inpVal == "") {
            InputFilled = false;
            return false;
        }
    })
    return InputFilled;
}
