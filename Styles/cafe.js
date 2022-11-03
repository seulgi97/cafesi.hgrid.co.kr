
$(function () {
    $("[id*=gv] td").hover(function () {
        $("td", $(this).closest("tr")).addClass("hover_row");
    }, function () {
        $("td", $(this).closest("tr")).removeClass("hover_row");
    });

    $('#networkcircle.circle').hover(function (e) {
        $('div#networkhelp').show();
    }, function () {
        $('div#networkhelp').hide();
    });

    $('#networkcircle.circle').mousemove(function (e) {
        /* $("div#networkhelp").css('top', e.pageY + moveDown).css('left', e.pageX + moveLeft);*/
    });

    $('#rackcircle.circle').hover(function (e) {
        $('div#rackhelp').show();
    }, function () {
        $('div#rackhelp').hide();
    });

    $('#rackcircle.circle').mousemove(function (e) {
        /* $("div#rackhelp").css('top', e.pageY + moveDown).css('left', e.pageX + moveLeft);*/
    });
});
