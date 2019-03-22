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
    for (var i = 0; i < cart.length; i++)
    {
        qty += parseInt(cart[i]['Quantity']);
    }
    $('#cartQty').html(qty);
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