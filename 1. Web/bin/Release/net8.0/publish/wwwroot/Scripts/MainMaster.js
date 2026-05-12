
function popwindow(url) {
var width = screen.width * 90 / 100;
var height = screen.height * 90 / 100;
var XPost = screen.width * 5 / 100;
var YPost = screen.height * 5 / 100;
window.open(url, "", "toolbar=0, scrollbars=1, location=0, statusbar=0, menubar=0, resizable=0, " +
        "width=" + width + ", height=" + height + ", left=" + XPost + ", top=" + YPost);
}

//Form Submission of Enter Key Press
function RKey(evt) {
    var evt = (evt) ? evt : ((event) ? event : null);
    var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
    if ((evt.keyCode == 13) && (node.type == "text")) { 
        var header = document.getElementById("ctl00_ContentPlaceHolder1_UCHeader_btnSearch");
        var search = document.getElementById("ctl00_ContentPlaceHolder1_UCSearch_btnSubmit");
        if (header) {header.click();return false;}
        if (search) {search.click();return false;}
           
        header = document.getElementById("ContentPlaceHolder1_UCHeader_btnSearch");
        search = document.getElementById("ContentPlaceHolder1_UCSearch_btnSubmit");
        if (header) {header.click();return false;}
        if (search) {search.click();return false;}
              
        }
            
}
document.onkeypress = RKey;
//
    
  
    $(function() {
       
$('.ROnly').keydown(function(event) {
        event.preventDefault(); 
}); 
    
$('.PMax3D').keyup(function () {
    var val = this.value, sign = '';
    if(val.lastIndexOf('-', 0) === 0){
        sign = '-';
        val = val.substring(1);
    }
    var parts = val.split('.').slice(0,2);
        if(parts[0] && parseInt(parts[0], 10).toString() !== parts[0]){
        parts[0] = parseInt(parts[0], 10);
        if(!parts[0])
            parts[0] = 0;
    }
    var result = parts[0];
    if(parts.length > 1){
        result += '.';
        if(parts[1].length > 3 || 
            parseInt(parts[1], 10).toString() !== parts[1]){
                parts[1] = parseInt(parts[1].substring(0,3), 10);
                if(!parts[1])
                    parts[1] = 0;
        }
        result += parts[1];
    }
    this.value = result;
                
});

$('.PMax2D').keyup(function () {
    var val = this.value, sign = '';
    if(val.lastIndexOf('-', 0) === 0){
        sign = '-';
        val = val.substring(1);
    }
    var parts = val.split('.').slice(0,2);
        if(parts[0] && parseInt(parts[0], 10).toString() !== parts[0]){
        parts[0] = parseInt(parts[0], 10);
        if(!parts[0])
            parts[0] = 0;
    }
    var result = parts[0];
                
    if(parts[0].length > 2 || 
            parseInt(parts[0], 10).toString() !== parts[0]){
                parts[0] = parseInt(parts[0].substring(0,2), 10);
                if(!parts[0])
                    parts[0] = "";
        }
    result = parts[0];
                
    if(parts.length > 1){
        result += '.';
        if(parts[1].length > 2 || 
            parseInt(parts[1], 10).toString() !== parts[1]){
                parts[1] = parseInt(parts[1].substring(0,2), 10);
                if(!parts[1])
                    parts[1] = 0;
        }
        result += parts[1];
    }
    this.value = result.toFixed(2);
                
});


$('.no1_9').keyup(function () {
    var yourInput = $(this).val();
        re = /[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi;
    var isSplChar = re.test(yourInput);
    if (isSplChar) {
        var no_spl_char = yourInput.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '');
        $(this).val(no_spl_char);
    };
                
    if (yourInput ==0) {
        var no_spl_char = '';
        $(this).val(no_spl_char);
    };
});
$('.no1_20').keyup(function () {
    var yourInput = $(this).val();
        re = /[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi;
    var isSplChar = re.test(yourInput);
    if (isSplChar) {
        var no_spl_char = yourInput.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '');
        $(this).val(no_spl_char);
    };
 
    if (!(yourInput>= 1 && yourInput <= 20)){
        var no_spl_char = '';
        $(this).val(no_spl_char);
    }
});

    $('.Formula').keydown(function(event) {
        if ( 
            event.keyCode == 46 || event.keyCode == 8 || 
            event.keyCode == 110 || event.keyCode == 190 || 
            event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || 
            (event.keyCode == 65 && event.ctrlKey === true) || 
            (event.keyCode == 189 && event.shiftKey === false) ||
            (event.keyCode == 187 && event.shiftKey === true) || 
            (event.keyCode == 53 && event.ctrlKey === false) || 
            (event.keyCode == 54 && event.ctrlKey === false) ||
            (event.keyCode == 56 && event.shiftKey === true) || 
            (event.keyCode == 57 && event.shiftKey === true) ||
            (event.keyCode == 48 && event.shiftKey === true) ||
            (event.keyCode == 191 && event.shiftKey === false) || 
            (event.keyCode >= 35 && event.keyCode <= 39)
            ) 
        {
                return;
        }
        else {
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105 )) {
                event.preventDefault(); 
            }   
        }
    });
  
    });    


$(function() {
$('.IntOnly').keydown(function(event) {
    if ( event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 109 || event.keyCode == 189 || event.keyCode == 110 || event.keyCode == 190 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || 
        (event.keyCode == 65 && event.ctrlKey === true) || 
        (event.keyCode >= 35 && event.keyCode <= 39)) {
            return;
    }
    else {
        if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105 )) {
            event.preventDefault(); 
        }   
    }
});
    
$('.NoDecimal').keydown(function(event) {
    if ( event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 109 || event.keyCode == 189 || event.keyCode == 110 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || 
        (event.keyCode == 65 && event.ctrlKey === true) || 
        (event.keyCode >= 35 && event.keyCode <= 39)) {
            return;
    }
    else {
        if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105 )) {
            event.preventDefault(); 
        }   
    }
});
  
$('.nosymbol').keyup(function () {
    var yourInput = $(this).val();
    re = /[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi;
    var isSplChar = re.test(yourInput);
    if (isSplChar) {
        var no_spl_char = yourInput.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '');
        $(this).val(no_spl_char);
    }
});


$('.NoSpChar').keyup(function () {
    var yourInput = $(this).val();
    var iChars = "!@#$%^&*'" + '"';
    for (var i = 0; i < yourInput.length; i++) {
        if (iChars.indexOf(yourInput.charAt(i)) != -1) {
                    
        yourInput = yourInput.slice(0, -1);
        $(this).val(yourInput);
        }
    }
});  
       
    $('.no-symbol').keyup(function () {
            var yourInput = $(this).val();
            re = /[`~!@#$%^&*()_|+\=?;:'",.<>\{\}\[\]\\\/]/gi;
            var isSplChar = re.test(yourInput);
            if (isSplChar) {
                var no_spl_char = yourInput.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '');
                $(this).val(no_spl_char);
            }
        });
    });

    function validateFloatKeyPress(el) {
        var v = parseFloat(el.value);
        el.value = (isNaN(v)) ? '' : v.toFixed(7);
    }
        
    function validateRMKeyPress(el) {
        var v = parseFloat(el.value);
        el.value = (isNaN(v)) ? '' : v.toFixed(4);
    }

   
    
$(function () {
    //if ($(".datepickerFrom").val() == "") {
    //    $('.datepickerFrom').datepicker({ format: 'yyyy-mm-dd' }).datepicker("setDate", new Date());
    //}
    //if ($(".datepickerTo").val() == "") {
    //    $('.datepickerTo').datepicker({ format: 'yyyy-mm-dd' }).datepicker("setDate", new Date());
    //}

    $(".datepickerFrom").datepicker({
        todayBtn: 1,
        format: 'yyyy-mm-dd',
        autoclose: true,
    }).on('changeDate', function (selected) {    
        var minDate = new Date(selected.date.valueOf());
        $('.datepickerTo').datepicker('setStartDate', minDate);
        }).attr('readonly', 'true');

    

    $(".datepickerTo").datepicker({
        format: 'yyyy-mm-dd',
        changeMonth: true,
        changeYear: true,
        autoclose: true,
    }).on('changeDate', function (selected) {
        var maxDate = new Date(selected.date.valueOf());
        $('.datepickerFrom').datepicker('setEndDate', maxDate);
        }).attr('readonly', 'true');


});
     
        function SetSelection() {
        $( ".divCover" ).removecss;
        window.parent.parent.scrollTo(0,0);
        window.top.divunlockscreen();
        }
         
         
         
        $(function() {
               
        $(".Monthpicker").datepicker({
            dateFormat: 'mm/yy',
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            onClose: function(dateText, inst) {
            var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            $(this).datepicker('setDate', new Date(year, month, 1));
            $(".Monthpicker2").datepicker("option", "minDate",  new Date(year, month, 1));
            }
        }).attr('readonly','true');
         
        $(".Monthpicker2").datepicker({
            dateFormat: 'mm/yy',
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            onClose: function(dateText, inst) {
            var month2 = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            var year2 = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            $(this).datepicker('setDate', new Date(year2, month2, 1));
            }
        }).attr('readonly','true');
         
         
         
            
        });
         
    var oricolor;
function Highlight(row) {
    oricolor = row.style.backgroundColor;
    row.style.backgroundColor = '#52B3A2';
}
    
function UnHighlight(row) {
    row.style.backgroundColor = oricolor
    //row.style.backgroundColor = '#FFFFFF';
}
    
//function setclass()
//{
//    $("input[type=text]").addClass("form-control");
//    $("select").addClass("form-control");
//    $("input[type=button]").addClass("btn btn-primary");
//    $("button").addClass("btn btn-primary");
//    $("input[type=submit]").addClass("btn btn-primary");
//    $("table").addClass("mdl-data-table mdl-js-data-table");
//    $("table").addClass("tblstyle");
//}

//    $(document).ready(function(){
       
//        setclass();
//        window.top.resizeIframe(document.body.scrollHeight);
//        if ($('#ctl00_ContentPlaceHolder1_btnreject').length > 0){
//            $('#ctl00_ContentPlaceHolder1_btnreject').click(function(){
//                var rs = confirm("Are you sure to reject?");
//                if (rs == true) {
//                    return true;
//                }
//                else {
//                    return false;
//                }
//            });
//        };   
//        if ($('#ctl00_ContentPlaceHolder1_btnapprove').length > 0){
//            $('#ctl00_ContentPlaceHolder1_btnapprove').click(function(){
//                var rs = confirm("Are you sure to Approve?");
//                if (rs == true) {
//                    return true;
//                }
//                else {
//                    return false;
//                }
//            });
//        };   
       
              
//    });
    

function ProcConfirm()
{
if (confirm('Are you sure to proceed ?')==true){
    window.top.divlockscreen();
    return true;
} 
else
    return false;
}

function ChkSubmit()
{
        
    if (typeof (Page_ClientValidate) == 'function') {
        Page_ClientValidate();
    }
    else{
        window.top.divlockscreen();
        return true;   
    }
        
    if (Page_IsValid) {
        window.top.divlockscreen();
        return true;                
    }
    else {
            return false;
    }       
}
