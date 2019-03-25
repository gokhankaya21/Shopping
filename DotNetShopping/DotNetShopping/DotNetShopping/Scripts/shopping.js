function loadShoppingCart() {
    $.post('/Api/GetShoppingCart')
        .done(function (response, status, jqxhr) {
            //alert(response['Success']);
            displayShoppingCart(response['Cart']);
        })
        .fail(function (jqxhr, status, error) {
            alert(response['Error!']);
        });
}

function displayShoppingCart(cart) {
    var qty = 0;
    var cartContent = "";
    for (var i = 0; i < cart.length; i++) {
        qty += parseInt(cart[i]['Quantity']);
        cartContent += getCartContent(cart[i]);
    }
    $('#cartQty').html(qty);
    $('#cartContent').html(cartContent);
}
function getCartContent(item) {
    var content = '<div class=\'cartItem\'>' +
        '<img src=\'../../ProductImage/' + item['PhotoName'] + '-1.jpg\'>' +
        '<div class=\'cartItemName\'>' + item['VariantName'] + ' ' + item['ProductName'] + '</div>' +
        '<div class=\'cartQuantity\'>' + item['Quantity'] + ' ' + 'x' + ' ' + '$' + item['UnitPrice'] + '</div>' +
        '<i class=\'cartRemove glyphicon glyphicon-remove-circle\' onclick=\'removeCart(' + item['VariantId'] + ');\' title=\'Delete\'></i>'
        + '</div>';
    return content;
}
function removeCart(variantId) {
    //alert(variantId + ' removed');
    var dataToPost = {
        VariantId: variantId
    };
    $.post('/Api/RemoveCart', dataToPost)
        .done(function (response, status, jqxhr) {
            //alert(response['Success']);
            displayShoppingCart(response['Cart']);
        })
        .fail(function (jqxhr, status, error) {
            //alert(response['Error!']);
        });
}

function addToCart(variantId, qty) {
    var dataToPost = {
        VariantId: variantId,
        Qty: qty
    };
    $.post('/Api/AddToCart', dataToPost)
        .done(function (response, status, jqxhr) {
            //alert(response['Success']);
            displayShoppingCart(response['Cart']);
        })
        .fail(function (jqxhr, status, error) {
            alert(response['Error!']);
        });
}
