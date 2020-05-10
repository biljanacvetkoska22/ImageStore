$(document).ready(function () {//when doc is ready we load

    var theForm = $("#theForm");
    theForm.hide();

    /*var button = document.getElementById("buyButton");
    button.addEventListener("click", function () {
        console.log("Buy Item")
    });*/

    var button = $("#buyButton");
    button.on("click", function () {
        console.log("Buying item");
    })

    /*var productInfo = document.getElementById("product-props");*/
    var productInfo = $(".product-props li");
    //var listItems = productInfo.item[0].children;
    productInfo.on("click", function () {
        console.log("You clicked on " + $(this).text());
    });

    var $loginToggle = $("#loginToggle");
    var $popupForm = $(".popup-form");

    $loginToggle.on("click", function () {
        $popupForm.fadeToggle(100);
    });   

}); // we wrap because of problem of colision and as soon is it is read we execute

