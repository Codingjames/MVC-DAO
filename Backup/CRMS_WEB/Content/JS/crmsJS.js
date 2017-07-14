// link ref 1 {ConfirmDel}
var goto = function (link) {
    window.location = link;
    console.log(link);
}

var ConfirmDel = function (link) {
    var msg = confirm("ต้องการลบข้อมูลนี้หรือไม่ ?");
    if (msg) {
        goto(link);
    }
    else {
        return false;
    }
}

// เฉพาะตัวเลข
// วิธีใช้ :  <input type="text" onkeypress="return isNumber(event,this);" />
var isNumber = function (e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if (t.value == "") {
            if (charCode > 31 && (charCode < 49 || charCode > 57)) {
                return false;
            } else {
                return true;
            }
        } else {
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            } else {
                return true;
            }
        }

        return true;
    }
    catch (err) {
        alert(err.Description);
    }
}

var isNumberStr = function (e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }

        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        } else {
            return true;
        }
        return true;
    }
    catch (err) {
        alert(err.Description);
    }
}

var isDuplicateUser = function (t) {
    if (t.value !== "" && t.value.length >= 4) {
        $.ajax({
            url: "../User/IsDuplicateUser",
            method :"post",
            data: { "userName": t.value },
            beforeSend: function () {
                $("#dupLD").empty();
                $("#dupLD").removeClass();
                //$("#dupLD").removeClass("glyphicon glyphicon-remove");
                $("#dupLD").html("<img src='../Content/images/loading2.gif' style='width:30px;height:30px;'/>");
                $("#dupLD").css({ "color": "green" });
            },
            success: function (data) {
                if(data.result === "fail"){
                    $("#dupLD").empty();
                    $("#dupLD").addClass("glyphicon glyphicon-ok");
                    $("[type='submit']").removeAttr("disabled");
                } else {
                    $("#dupLD").empty();
                    $("#dupLD").addClass("glyphicon glyphicon-remove");
                    $("#dupLD").css({ "color": "red" });
                    $("[type='submit']").attr("disabled", "disabled");
                }

            }
        });
    } else {
        $("#dupLD").empty();
        $("#dupLD").removeClass();
    }
};

// password check
var pwdCheck = function (idInput1, idInput2) {
    var p1 = $("#"+idInput1);
    var p2 = $("#"+idInput2);
    if (p1.val()===p2.val()) {
        return true;
    } else {
        p2.val('');
        p2.focus();
        alert(" รหัสผ่านไม่ถูกต้อง ");
        return false;
    }
};
var newPwdCheck = function (idInput1, idInput2) {
    var p1 = $("#" + idInput1);
    var p2 = $("#" + idInput2);

    if(p1.val() != "" ){
        if (p1.val() === p2.val()) {
            return true;
        } else {
            p2.val('');
            p2.focus();
            alert(" รหัสผ่านไม่ถูกต้อง ");
            return false;
        }
    }
    else if (p1.val() == "") {
        if (p2.val() == p1.val()) {
            return true;
        } else {
            p1.focus();
            alert(" รหัสผ่านไม่ถูกต้อง ");
            return false;
        }
    }
else {
        return true;
}

    
};

var badgeAlertBell = function (cid) {
    $.post("../Services/DataLoading_BadgeAlertBell", { "c": cid }, function (data, status) {
        if (status == "success") {
            console.log(data);
            $("#alertBell").html(data.data);
        }
    });
}
var badgeNews = function (cid) {
    $.post("../SysMassage/DataLoading_BadgeMSG", { "c": cid }, function (data, status) {
        if (status == "success") {
            console.log("badgeNews statuse = " + data.data);

            $("#badgeMSG").html(data.data);
        }
    });
}

/// เฉพาะ Eng and number 
var engAndNum = function (event) {
    var ew = event.which;
    if (ew == 32)
        return true;
    if (48 <= ew && ew <= 57)
        return true;
    if (65 <= ew && ew <= 90)
        return true;
    if (97 <= ew && ew <= 122)
        return true;
    return false;
}