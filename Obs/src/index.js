import Foo from "./foo"

class Site {
    constructor() {
        this.Foo = new Foo();
        this.ActivateReadonlies();
    }
    ActivateReadonlies() {
        $(".readonly").on('keydown paste', function(e){
            e.preventDefault();
        });
    }
}
// to handle enter press
$(function () {
    $('input').keydown(function (e) {
        if (e.keyCode == 13) {
            $("input[value='OK']").focus().click();
            return false;
        }
    });
});

window.Site = new Site();