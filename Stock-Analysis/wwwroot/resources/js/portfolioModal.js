$(document).ready(function () {
    $("#myBtn").click(function () {
        $("#myModal").modal('show');
    });

    $(".close").click(function () {
        $("#myModal").modal('hide');
    });
});

$(window).click(function (event) {
    if (event.target == $("#myModal")[0]) {
        $("#myModal").modal('hide');
    }
});
function submitPortfolio() {
    var quantity = $('#quantity').val();
    var price = $('#price').val();
    var ticker = '@ticker';
    var market = '@market';

    $.ajax({
        type: "POST",
        url: "/portfolio/add",
        data: { quantity: quantity, price: price, ticker: ticker, market: market },
        success: function (response) {
            if (response.success) {
                alert('포트폴리오에 추가되었습니다.');
                $('#myModal').modal('hide');
            } else {
                alert(response.message || '에러가 발생했습니다.');
            }
        },
        error: function () {
            alert('통신 중 에러가 발생했습니다.');
        }
    });
}