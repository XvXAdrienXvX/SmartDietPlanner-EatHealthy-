var ActArr = [];
var foodArr = [];
var selectedAct = [];
function loaded() {
    var FoodDataList="";
    var loopcounter = 0;
    for (loopcounter = 0; loopcounter < exerciseData.length; loopcounter++) {
        ActArr.push({
            name: exerciseData[loopcounter], abbreviation: exerciseID[loopcounter] });
    }
    
    for (loopcounter = 0; loopcounter < FoodArrJS.length; loopcounter++) {

        FoodDataList += ' <option value="' + FoodArrJS[loopcounter] +'-'+ FoodMacro[loopcounter] + '  " />';
        foodArr.push({
            name: FoodArrJS[loopcounter], abbreviation: FoodIDArrJS[loopcounter]
        });
    }
    populateFood();
    document.getElementById("FoodsDB").innerHTML = FoodDataList;
    populateAct();
}
var arr = ["acc", "err","fee"];
$('.dropdown-container')
	.on('click', '.dropdown-button', function() {
        $(this).siblings('.dropdown-list').toggle();
	})
	.on('input', '.dropdown-search', function() {
    	var target = $(this);
        var dropdownList = target.closest('.dropdown-list');
    	var search = target.val().toLowerCase();

    	if (!search) {
            dropdownList.find('li').show();
            return false;
        }

    	dropdownList.find('li').each(function() {
        	var text = $(this).text().toLowerCase();
            var match = text.indexOf(search) > -1;
            $(this).toggle(match);
        });
	})
	.on('change', '[type="checkbox"]', function() {
        var container = $(this).closest('.dropdown-container');
        var numChecked = container. find('[type="checkbox"]:checked').length;
    	container.find('.quantity').text(numChecked || 'Any');
	});

// JSON of States for demo purposes


var Allergies = [
    { name: 'MILK', abbreviation: 'MILK' },
    { name: 'EGGS', abbreviation: 'EGGS' },
    { name: 'NUTS', abbreviation: 'NUTS' },
    { name: 'SOY', abbreviation: 'SOY' },
    { name: 'FISH', abbreviation: 'FISH' },
    { name: 'SHELLFISH', abbreviation: 'SHELLFISH' }
];

// <li> template

var allergiesTemplate = _.template(
    '<li>' +
    '<input class ="AllergyChoice" name="<%= abbreviation %>" type="checkbox">' +
    '<label for="<%= abbreviation %>"><%= capName %></label>' +
    '</li>'
);

function populateAct() {
    var ActTemplate = _.template(
        '<li>' +
        '<input onclick = "ActSelected(this, \'<%= abbreviation %>\', \'<%= capName %>\')" class ="ActivityChoice" name="<%= abbreviation %>" type="checkbox">' +
        '<label for="<%= abbreviation %>"><%= capName %></label>' +
        '</li>'
    );

    // Populate list with activities
    _.each(ActArr, function (s) {
        s.capName = _.startCase(s.name.toLowerCase());
        $('.ActivitiesList').append(ActTemplate(s));
    });
}

function populateFood() {
    var FoodTemplate = _.template(
        '<li>' +
        '<input id="<%= abbreviation %>_"   onclick = "FoodSelected(this, \'<%= abbreviation %>\', \'<%= capName %>\')"  name="<%= abbreviation %>" type="checkbox">' +
        '<label for="<%= abbreviation %>"><%= capName %></label>' +
        '</li>'
    );

    // Populate list with activities
    _.each(foodArr, function (s) {
        s.capName = _.startCase(s.name.toLowerCase());
        $('.FoodList').append(FoodTemplate(s));
    });
}

function FoodSelected(elem, id, name) {
    if (elem.checked == true) {
        AddFoodFields(name, id);
    }
    else {
        RemoveFoodFields(id);
    }
}

function AddFoodFields(name, id) {

    var ActDiv = document.getElementById("FoodFields");

    ActDiv.innerHTML += "<div class='form-group col-lg-6 " + id + "'> <label for='" + id + "'>Quantity of " + name + " Consumed</label> <input type='number' value='1' name='" + name + "' class='form-control  ExerciseDuration' id='" + id + "' placeholder='Enter quantity' step='1' min='1'> </div>";

    var ChipsDiv = document.getElementById("ChipRow");

    ChipsDiv.innerHTML += " <div class='chip '>" + name + " <span class='closebtn' id= _"+id+"  onclick='RemoveChips(this, "+id+")'>&times;</span> </div>" ;

}

function RemoveChips(Elem,id) {
    Elem.parentElement.style.display = 'none';
    $('.' + id).remove();
    document.getElementById(id + "_").checked = false;
    document.getElementById("Quan").innerHTML -= 1;


}

function RemoveFoodFields(id) {

    $('.' + id).remove();

    document.getElementById( "_" +id ).parentElement.style.display = 'none';
}

function SetFood() {
    var ActList = "";
    var EmptyField = false;
    var GoodToSubmit = false;
    $('.ExerciseDuration').each(function (index, obj) {
        if (obj.value == "" || obj.value == " ") {
            obj.style.border = "2px solid red";
            EmptyField = true;
        }
        else {
            obj.style.border = "1px solid #CED3D9";

        }

    });

    if (EmptyField == false) {
      
        GoodToSubmit = true;
        document.getElementById("EmptyFieldErr").innerHTML = "";

        $('.ExerciseDuration').each(function (index, obj) {

            document.getElementById("ActivitiesDone").innerHTML += "<li title='Duration: " + obj.value + " mins' style='cursor:default'><i class='menu-icon fas fa-circle'></i><a>" + obj.name + "</a></li>";
            ActList += obj.id + "=" + obj.value;
            ActList += ",";

        });
    }
    else {
        GoodToSubmit = false;

        document.getElementById("EmptyFieldErr").innerHTML = " Please fill all the fields to proceed";

    }

    if (GoodToSubmit == true) {
        ActList = ActList.slice(0, -1);
        $.ajax({
            type: "get",
            url: "/user/SetDailyConsumption",

            data: {

                'ConsumptionList': ActList
            },
            success: function (data) {
                var loopc = 0;
                var remains = 0;
                $.map(data, function (item) {
                    if (loopc === 0) {
                        alert(loopc + "==" + item);
                        document.getElementById("ConsumedCal").innerHTML = "Calories Consumed: " + item;
                         remains = (cals - item);
                        if (remains < 0)
                            remains = 0;
                        document.getElementById("RemainingCal").innerHTML = "Calories Remaining: " + remains.toFixed(2);
                        loopc++;
                    }
                   else if (loopc === 1) {
                        alert(loopc + "===" + item);

                         remains = (proteinJS - item);
                        if (remains < 0)
                            remains = 0;
                        document.getElementById("ProteinRemain").innerHTML = remains.toFixed(2);
                        loopc++;
                    }
                  else  if (loopc === 2) {
                        alert(loopc + "====" + item);

                         remains = (carbJS - item);
                        if (remains < 0)
                            remains = 0;
                        document.getElementById("CarbRemain").innerHTML = remains.toFixed(2);
                        loopc++;
                    }
                    else {
                         remains = (fatJS - item);
                        if (remains < 0)
                            remains = 0;
                        document.getElementById("FatRemain").innerHTML = remains.toFixed(2);
                    }
                }
                );
                $("#toggleActModal").click();
            }     
        });

    }

}
// Populate list with states
_.each(Allergies, function (s) {
    s.capName = _.startCase(s.name.toLowerCase());
    $('.AllergiesList').append(allergiesTemplate(s));
});

function AllergyChange(){
var boolAllergy = document.getElementById("AllergiesCheck").checked;
if(boolAllergy== false)
disableDrop();
else
enableDrop();
}

function disableModel() {


    $("#toggleModal").click();

}
  function disableDrop() {

    var allergyDrop = document.getElementById("Dropallergies");
    allergyDrop.disabled= true;
      $("#Dropallergies").css("background-color", "#CBCBCB");
      $("#Dropallergies").css("cursor", "not-allowed");
    $("#AllergiesLists").css("display", "none");


  }

function enableDrop(){
    var allergyDrop = document.getElementById("Dropallergies");
  allergyDrop.disabled= false;
    $("#Dropallergies").css("background-color", "whitesmoke");
    $("#Dropallergies").css("cursor", "pointer");
}


function previewFile(){
    var preview = document.getElementById("Prev"); //selects the query named img
    var file    = document.getElementById("up").files[0]; //sames as here
    var reader  = new FileReader();

    reader.onloadend = function () {
        preview.src = reader.result;
    };

    if (file) {
        reader.readAsDataURL(file); //reads the data as a URL
    } 
}

function checkPWD() {
    alert("a");
    var pwd = document.getElementById("UserPassword").value;
    var cpwd = document.getElementsById("cpass").value;
    alert(pwd);
    if (pwd != cpwd) {
        document.getElementById("errMessage").innerHTML = " <div class='alert  alert-danger alert-dismissible fade show' role='alert'>Password don't Match!</div>";
        return false;
    }
    else
        document.getElementById("errMessage").innerHTML = "";

}

function ActSelected(elem,id,name) {
    if (elem.checked == true) {
        AddActFields(name, id);
    }
    else {
        RemoveActFields(id);
    }
}

function AddActFields(name, id) {

    var ActDiv = document.getElementById("ActFields");

    ActDiv.innerHTML += "<div class='form-group col-lg-6 " + id + "'> <label for='" + id + "'>Duration for: " + name + "</label> <input type='number' name='"+name+"' class='form-control  ExerciseDuration' id='" + id + "' placeholder='Enter Duration in minutes' step='5' min='0'> </div>";
}

function RemoveActFields(id) {

    $('.' + id).remove();

}

function SetActivities() {
    var ActList = "";
    var EmptyField = false;
    var proceedToSubmit = false;
    $('.ExerciseDuration').each(function (index, obj) {
        if (obj.value == "" || obj.value == " ") {
            obj.style.border = "2px solid red";
            EmptyField = true;
        }
        else {
            obj.style.border = "1px solid #CED3D9";

        }

    });

    if (EmptyField == false) {
        ActList = "";

        proceedToSubmit = true;
        document.getElementById("EmptyFieldErr").innerHTML = "";

        $('.ExerciseDuration').each(function (index, obj) {

            document.getElementById("ActivitiesDone").innerHTML += "<li title='Duration: " + obj.value + " mins' style='cursor:default'><i class='menu-icon fas fa-circle'></i><a>" + obj.name + "</a></li>";
            ActList += obj.id + "=" + obj.value;
            ActList += ",";

        });
    }
    else {
        proceedToSubmit = false;

        document.getElementById("EmptyFieldErr").innerHTML = " Please fill all the fields to proceed";

    }

    if (proceedToSubmit == true) {
        ActList = ActList.slice(0, -1);
        $.ajax({
            type: "get",
            url: "/User/SetActivities",

            data: {

                'ActivitiesList': ActList
            },
            success: function () {
                $("#toggleActModal").click();


            }
        });

    }

}


var Preferences = [
    { name: 'Dairy', abbreviation: 'Dairy' },
    { name: 'Chicken', abbreviation: 'Chicken' },
    { name: 'Fruits', abbreviation: 'Fruit' },
    { name: 'Salads', abbreviation: 'Salad' },
    { name: 'Wholegrains', abbreviation: 'Wholegrain' },
    { name: 'Seafood', abbreviation: 'Seafood' }
];

// <li> template

var preferencesTemplate = _.template(
    '<li>' +
    '<input class ="preferenceChoice" name="<%= abbreviation %>" type="checkbox">' +
    '<label for="<%= abbreviation %>"><%= capName %></label>' +
    '</li>'
);



// Populate list with states
_.each(Preferences, function (s) {
    s.capName = _.startCase(s.name.toLowerCase());
    $('.PreferencesList').append(preferencesTemplate(s));
});

function preferenceChange() {
    var boolAllergy = document.getElementById("PreferencesCheck").checked;
    if (boolAllergy == false)
        disableDrop();
    else
        enableDrop();
}

