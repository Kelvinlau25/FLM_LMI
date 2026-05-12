$(function ($) {

    var w = window.innerWidth;
    if (w > 800) {
        $('.RightBar').removeClass("full");
    } else {
        $('.RightBar').addClass("full");
        $('.maincover.LeftBar').removeClass("in");
        $('.navbar-collapse.collapse').removeClass("in");
        $('.full-height .maincover.collapse').removeClass("in");
    }
    //$('#make-small-nav').click(function (e) {
    //    if ($('.RightBar').hasClass("HideLeftBar")) {
    //        $('.RightBar').removeClass("HideLeftBar");
    //        $('.LeftBar').removeClass("LeftNone");
    //    } else {
    //        $('.RightBar').addClass("HideLeftBar");
    //        $('.LeftBar').addClass("LeftNone");
    //    }
    //});
    $('.navbar-toggle').click(function (e) {

        if ($('.maincover.LeftBar').hasClass("in")) {
            $('.navbar-collapse.collapse').addClass("in");
            $('.maincover.LeftBar').removeClass("in");
            $('.RightBar').addClass("full");
        } else {
            $('.RightBar').removeClass("full");
            $('.maincover.LeftBar').addClass(" in");
            $('.navbar-collapse.collapse').removeClass("in");


        }
    });

    $(document).ready(function () {
        
        //$('input').on('paste', function (e) {
        //    // common browser -> e.originalEvent.clipboardData
        //    // uncommon browser -> window.clipboardData
        //    var clipboardData = e.originalEvent.clipboardData;
        //    alert(e.originalEvent.clipboardData.Text);
        //});
        $(".onlyNumber").forceNumeric();
        $(".onlyDouble").forceDouble();

    });
    
    jQuery.fn.forceNumeric = function () {
        return this.each(function () {
            var ctrlDown = false,
                ctrlKey = 17,
                cmdKey = 91,
                vKey = 86,
                cKey = 67;

            $(this).keydown(function (e) {
                if (e.keyCode == ctrlKey || e.keyCode == cmdKey) ctrlDown = true;
            }).keyup(function (e) {
                if (e.keyCode == ctrlKey || e.keyCode == cmdKey) ctrlDown = false;
             });

            $(this).keydown(function (e) {
                var x = e.code;
             //   alert(e.keyCode);
                if (x.substring(0, 5) == "Digit" || x == "Backspace" || x == "Delete" || x == "Tab" || x == "ArrowLeft" || x == "ArrowRight") {
                    if (this.value.includes("ArrowLeft") && e.key == "ArrowLeft") {
                        return false;
                    }
                    if (this.value.includes("ArrowRight") && e.key == "ArrowRight") {
                        return false;
                    }
                    return true;
                }           
                else {
                    if (ctrlDown && (e.keyCode == vKey)) {
                        // alert("X");
                      //  $(this).value = ""
                    //  var clipboardData = e.clipboardData || e.originalEvent.clipboardData || window.clipboardData;
                      //  $(this).value(clipboardData.getData('text'));
                        return true;
                    }else if (x == "Enter") {
                        submitForm();
                    }
                    return false;
                }
                  
            });

            $(this).focus(function (e) {
                if (this.value == "0")
                    this.value = "";

                return true;
            });

            $(this).focusout(function (e) {
                if (this.value == "")
                    this.value = "0";

                return true;
            });
        });
    }

    jQuery.fn.forceDouble = function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var x = e.code;
               // alert(x);
                if (x.substring(0, 5) == "Digit" || e.key == "." || x == "Backspace" || x == "Delete" || x == "Tab" || x == "ArrowLeft" || x == "ArrowRight") {
                    if (this.value.includes(".") && e.key == ".") {
                        return false;
                    }
                    if (this.value.includes("ArrowLeft") && e.key == "ArrowLeft") {
                        return false;
                    }
                    if (this.value.includes("ArrowRight") && e.key == "ArrowRight") {
                        return false;
                    }
                    return true;
                }
                else
                    if (x == "Enter")
                        submitForm();
                return false;
            });

            $(this).focus(function (e) {
                if (this.value == "0" || this.value == "0.00")
                    this.value = "";

                return true;
            });

            $(this).focusout(function (e) {
                if (this.value == "")
                    this.value = "0.00";
            });
        });
    }
});